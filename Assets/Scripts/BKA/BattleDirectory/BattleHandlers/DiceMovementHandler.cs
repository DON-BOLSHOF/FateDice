using System;
using System.Collections.Generic;
using System.Threading;
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

        private async UniTask MoveDice(DiceObject dice, Vector3 position, CancellationToken token)
        {
            /*
            var euler = dice.transform.localEulerAngles;
            var test = euler / 90;
            */

            /*int x = ((int)Math.Round(test.x * 100))%100;
            x = x > 95 || x < 5 ? (int)Math.Round(test.y) * 90 : 0;   
            int y = ((int)Math.Round(test.y * 100))%100;
            y = y > 95 || y < 5 ? (int)Math.Round(test.y) * 90 : 0;    
            int z = ((int)Math.Round(test.z * 100))%100;
            z = z > 95 || z < 5 ? (int)Math.Round(test.z) * 90 : 0; 
            
            /*
            euler.x = (int)(euler.x)%90==0?euler.x:0;
            euler.y = (int)(euler.y)%90==0?euler.y:0;
            euler.z = (int)(euler.z)%90==0?euler.z:0;#1#

            test = new Vector3(x, y, z);*/

            var sequence = DOTween.Sequence().Append(dice.transform.DOMove(position, 0.65f))
                /*.Join(dice.transform.DOLocalRotate(test,0.65f))*/;

            await sequence.ToUniTask(cancellationToken: token);
        }

        public async UniTask MoveDicesToBase(List<UnitDice> activeDices, CancellationToken token = default)
        {
            _isMovementComplete.Value = false;
            
            activeDices.ForEach(dice => dice.DiceObject.DeactivatePhysicality());
            
            var uniTasks = activeDices.Select(unitDice => MoveDice(unitDice.DiceObject, unitDice.BaseUnitPosition, token));
            await UniTask.WhenAll(uniTasks);
            
            activeDices.ForEach(dice => dice.DiceObject.ActivatePhysicality());

            _isMovementComplete.Value = true;
        } 
        
        public async UniTask MoveDicesFromBase(List<UnitDice> activeDices, CancellationToken token = default)
        {
            _isMovementComplete.Value = false;
            
            activeDices.ForEach(dice =>
            {
                dice.DiceObject.DeactivatePhysicality();
                dice.DiceObject.transform.position = dice.BaseUnitPosition;
            });
            
            var uniTasks = activeDices.Select(unitDice => MoveDice(unitDice.DiceObject, unitDice.PositionInBoard, token));
            await UniTask.WhenAll(uniTasks);
            
            activeDices.ForEach(dice => dice.DiceObject.ActivatePhysicality());

            _isMovementComplete.Value = true;
        }
    }
}