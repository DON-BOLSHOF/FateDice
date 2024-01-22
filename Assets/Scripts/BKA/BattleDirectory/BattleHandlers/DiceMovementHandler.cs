using System;
using System.Collections.Generic;
using BKA.Dices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace BKA.BattleDirectory.BattleHandlers
{
    public class DiceMovementHandler : MonoBehaviour
    {
        private ReactiveProperty<bool> _isMovementComplete = new(true);
        public IReadOnlyReactiveProperty<bool> IsMovementComplete => _isMovementComplete;

        private async UniTask MoveDice(DiceObject dice, Vector3 position)
        {
            //await dice.transform.DOMove(position, 1);

            await UniTask.Delay(TimeSpan.FromSeconds(1));

            dice.transform.position = position;
            dice.transform.rotation = Quaternion.identity;
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
            
            activeDices.ForEach(dice => dice.DiceObject.transform.position = dice.BaseUnitPosition);
            
            foreach (var unitDice in activeDices)
            {
                await MoveDice(unitDice.DiceObject, unitDice.PositionInBoard);
            }

            _isMovementComplete.Value = true;
        }
    }
}