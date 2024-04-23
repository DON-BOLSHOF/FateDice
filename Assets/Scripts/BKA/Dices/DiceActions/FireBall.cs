using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/FireBall", menuName = "Additional/DiceAttribute/FireBall")]
    public class FireBall : DiceActionData
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        public override Action<UnitBattleBehaviour> Act => Fire;
        public override Action<UnitBattleBehaviour> Undo => UndoFire;
        
        private void Fire(UnitBattleBehaviour battleBehaviour)
        {
            battleBehaviour.Unit.ModifyHealth(-3);
        }
        
        private void UndoFire(UnitBattleBehaviour battleBehaviour)
        {
            battleBehaviour.Unit.ModifyHealth(3);
        }
    }
}