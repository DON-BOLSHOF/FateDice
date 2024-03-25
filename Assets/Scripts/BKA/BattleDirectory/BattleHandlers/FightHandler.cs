using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BKA.BattleDirectory.BattleSystems;
using BKA.BattleDirectory.ReadinessObserver;
using BKA.Dices.DiceActions;
using BKA.System;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class FightHandler : MonoBehaviour, ITurnSystemVisitor, IReadinessObservable
    {
        private readonly List<UnitBattleBehaviour> _firstPack = new();
        private readonly List<UnitBattleBehaviour> _secondPack = new();

        [Inject] private TurnSystem _turnSystem;

        [SerializeField] private DiceHandler _diceHandler;

        [Inject] private UnitBattleBehaviourUploader _behaviourUploader;

        [Inject] private ReadinessToBattleObservable _readinessObservable;

        public ReadOnlyReactiveProperty<bool> IsReady => _isReady.ToReadOnlyReactiveProperty();

        private ReactiveProperty<bool> _isReady = new(true);

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
            _isReady.Value = false;
            
            await UniTask.WaitUntil(() => _readinessObservable.IsReady.Value);
         
            var currentTurn = _turnSystem.TurnState.Value;
            
            await _diceHandler.HandleNextTurn(currentTurn);

            switch (currentTurn)
            {
                case TurnState.PartyTurn:
                    Debug.Log("Party");
                    await PlayerMove();
                    break;
                case TurnState.EnemyTurn:
                    Debug.Log("Enemy");
                    await EnemyMove();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _isReady.Value = true;
        }

        private async UniTask PlayerMove()
        {
        }

        private async UniTask EnemyMove()
        {
            foreach (var unitBattleBehaviour in _secondPack)
            {
                int targetIndex;
                switch (unitBattleBehaviour.DiceAction.DiceActionData.DiceAttributeFocus)
                {
                    case DiceAttributeFocus.None:
                        break;
                    case DiceAttributeFocus.Enemy:
                        targetIndex = Random.Range(0, _firstPack.Count);
                        unitBattleBehaviour.DiceAction.ChooseTarget(_firstPack[targetIndex]);
                        break;
                    case DiceAttributeFocus.Ally:
                        targetIndex = Random.Range(0, _secondPack.Count);
                        unitBattleBehaviour.DiceAction.ChooseTarget(_secondPack[targetIndex]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            await UniTask.Yield();
            
            foreach (var unitBattleBehaviour in _secondPack)
            {
                await unitBattleBehaviour.Act();
            }
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}