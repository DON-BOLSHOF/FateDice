using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/SwordAttack", menuName = "Additional/DiceAttribute/SwordAttack")]
    public class SwordAttack : DiceActionData
    { 
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }

        public override Action<UnitBattleBehaviour> Action => Attack;

        private void Attack(UnitBattleBehaviour unit)
        {
            unit.Unit.ModifyHealth(-4);
        }
    }
}