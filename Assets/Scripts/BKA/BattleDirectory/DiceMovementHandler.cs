using System.Collections.Generic;
using System.Threading.Tasks;
using BKA.Dices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace BKA.BattleDirectory
{
    public class DiceMovementHandler : MonoBehaviour
    {
        private ReactiveProperty<bool> _isMovementComplete = new(true);
        public IReadOnlyReactiveProperty<bool> IsMovementComplete => _isMovementComplete;

        public async UniTask MoveDicesToBase(DiceObject[] diceObjects, Vector3[] positions)
        {
            _isMovementComplete.Value = false;
            
            for (var i = 0; i < diceObjects.Length; i++)
            {
                await MoveDice(diceObjects[i], positions[i]);
            }

            _isMovementComplete.Value = true;
        }

        private async UniTask MoveDice(DiceObject dice, Vector3 position)
        {
            await dice.transform.DOMove(position, 2);
        }

        public async UniTask MoveDicesToBase(List<UnitDice> activeDices)
        {
            _isMovementComplete.Value = false;
            
            foreach (var unitDice in activeDices)
            {
                await MoveDice(unitDice.DiceObject, unitDice.BaseUnitPosition);
            }

            _isMovementComplete.Value = true;
        } 
        
        public async UniTask MoveDicesFromBase(List<UnitDice> activeDices)
        {
            _isMovementComplete.Value = false;
            
            foreach (var unitDice in activeDices)
            {
                await MoveDice(unitDice.DiceObject, unitDice.PositionInBoard);
            }

            _isMovementComplete.Value = true;
        }
    }
}