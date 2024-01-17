using System.Collections.Generic;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory
{
    public class FightHandler : MonoBehaviour, ITurnSystemVisitor
    {
        private List<UnitBattleBehaviour> _firstPack;
        private List<UnitBattleBehaviour> _secondPack;

        [Inject] private TurnSystem _turnSystem;

        [SerializeField] private DiceHandler _diceHandler;

        public void DynamicInit(List<UnitBattleBehaviour> teammates, List<UnitBattleBehaviour> enemy)
        {
            _firstPack = teammates;
            _secondPack = enemy;

            _turnSystem.TurnState.Subscribe(_ => StartBattle()).AddTo(this);
        }

        public async void StartBattle()
        {
            var currentTurn = _turnSystem.TurnState.Value;

            await _diceHandler.HandleNextTurn(currentTurn);
        }

        public void Accept(TurnSystem turnSystem)
        {
            turnSystem.Visit(this);
        }
    }
}