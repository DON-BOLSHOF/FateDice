using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/SwordAttack", menuName = "Defs/DiceAttribute/SwordAttack")]
    public class SwordAttack : DiceActionData
    { 
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }

        public override Action<UnitBattleBehaviour> Act => Attack;
        public override Action<UnitBattleBehaviour> Undo => UndoAttack;

        private void Attack(UnitBattleBehaviour unitBehaviour)
        {
            unitBehaviour.Unit.ModifyHealth(-4);
        }

        private void UndoAttack(UnitBattleBehaviour unitBehaviour)
        {
            unitBehaviour.Unit.ModifyHealth(4);
        }
    }
}