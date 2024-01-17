using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Dices;
using BKA.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace BKA.BattleDirectory
{
    public class DiceHandler : MonoBehaviour
    {
        private List<UnitDice> _partyDices;
        private List<UnitDice> _enemyDices;

        [SerializeField] private RerollHandler _rerollHandler;
        [SerializeField] private DiceMovementHandler _diceMovementHandler;

        public ReadOnlyReactiveProperty<bool> IsDiceHandlerCompleteWork;

        private void Start()
        {
            IsDiceHandlerCompleteWork = _rerollHandler.IsDicesReady
                .CombineLatest(_diceMovementHandler.IsMovementComplete, (isDicesReady, isMovementComplete) => isDicesReady && isMovementComplete)
                .ToReadOnlyReactiveProperty();
        }
        
        public void DynamicInit(IEnumerable<IUnitOfBattle> party, IEnumerable<IUnitOfBattle> enemy)
        {
            _partyDices = party.Select(unit => new UnitDice { DiceObject = unit.DiceObject, BaseUnitPosition = unit.Position}).ToList();
            _enemyDices = enemy.Select(unit => new UnitDice { DiceObject = unit.DiceObject, BaseUnitPosition = unit.Position}).ToList();
        }

        private void ChangeDices(TurnState turnState)
        {
            switch (turnState)
            {
                case TurnState.PartyTurn:
                    foreach (var unitDice in _enemyDices)
                    {
                        unitDice.DiceObject.gameObject.SetActive(false);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    _rerollHandler.UpdateDices(_partyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
                case TurnState.EnemyTurn:
                    foreach (var diceObject in _enemyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(true);
                    }

                    foreach (var diceObject in _partyDices)
                    {
                        diceObject.DiceObject.gameObject.SetActive(false);
                    }

                    _rerollHandler.UpdateDices(_enemyDices.Select(unitDice => unitDice.DiceObject).ToList());
                    break;
            }
        }

        public async UniTask HandleNextTurn(TurnState currentTurn)
        {
            List<UnitDice> activeDices = currentTurn switch
            {
                TurnState.PartyTurn => _partyDices,
                TurnState.EnemyTurn => _enemyDices,
                _ => throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null)
            };
            
            ChangeDices(currentTurn);
            await UniTask.Yield();
            await _diceMovementHandler.MoveDicesToBase(activeDices);
            /*await reroll*/

        }
    }

    public class UnitDice
    {
        public Vector3 PositionInBoard;
        public Vector3 BaseUnitPosition;
        public DiceObject DiceObject;
    }
}