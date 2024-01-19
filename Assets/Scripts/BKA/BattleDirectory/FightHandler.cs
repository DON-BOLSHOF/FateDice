using System;
using System.Collections.Generic;
using BKA.System;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory
{
    public class FightHandler : MonoBehaviour, ITurnSystemVisitor
    {
        private readonly List<UnitBattleBehaviour> _firstPack = new();
        private readonly List<UnitBattleBehaviour> _secondPack = new();

        [Inject] private TurnSystem _turnSystem;

        [SerializeField] private DiceHandler _diceHandler;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        [Inject] private ReadinessToBattleObserver _readinessObserver;

        private void Start()
        {
            _behaviourUploader.OnUploadedBehaviour.Subscribe(value => UpdateUnits(value.Item1, value.Item2))
                .AddTo(this);
            
            _turnSystem.TurnState.Subscribe(_ => StartBattle()).AddTo(this);
        }

        private void UpdateUnits(UnitBattleBehaviour unit, UnitSide side)
        {
            switch (side)
            {
                case UnitSide.Party:
                    _firstPack.Add(unit);
                    break;
                case UnitSide.Enemy:
                    _secondPack.Add(unit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public async void StartBattle()
        {
            var currentTurn = _turnSystem.TurnState.Value;
            
            await UniTask.WaitUntil(() => _readinessObserver.IsReadyToBattle.Value);

            await _diceHandler.HandleNextTurn(currentTurn);
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}