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
        public IReadOnlyReactiveProperty<bool> IsMovementComplete => _isMovementComplete;
        
        private ReactiveProperty<bool> _isMovementComplete = new(true);

        private int _countMovingDices = 0;

        public async UniTask MoveDiceToBase(UnitDice activeDice, CancellationToken token = default)
        {
            _countMovingDices++;
            activeDice.DiceObject.DeactivatePhysicality();
            
            await MoveDice(activeDice.DiceObject, activeDice.BaseUnitPosition, token);
            
            activeDice.DiceObject.ActivatePhysicality();
            _countMovingDices--; 
        }
       
        public async UniTask MoveDiceToPositionInBoard(UnitDice activeDice, CancellationToken token = default)
        {
            activeDice.DiceObject.DeactivatePhysicality();
            activeDice.DiceObject.transform.position = activeDice.BaseUnitPosition;

            await MoveDice(activeDice.DiceObject, activeDice.PositionInBoard, token);
            
            activeDice.DiceObject.ActivatePhysicality();
        }

        private void Update()
        {
            _isMovementComplete.Value = _countMovingDices <= 0;
        }

        private async UniTask MoveDice(DiceObject dice, Vector3 position, CancellationToken token)
        {
            var sequence = DOTween.Sequence().Append(dice.transform.DOMove(position, 0.65f));

            await sequence.ToUniTask(cancellationToken: token);
        }
    }
}