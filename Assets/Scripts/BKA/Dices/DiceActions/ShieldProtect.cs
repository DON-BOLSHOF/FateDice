using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/ShieldProtect", menuName = "Additional/DiceAttribute/ShieldProtect")]
    public class ShieldProtect : DiceActionData
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        public override Action<UnitBattleBehaviour> Act => ShieldUnit;
        public override Action<UnitBattleBehaviour> Undo => UnShieldUnit;

        private void ShieldUnit(UnitBattleBehaviour unitBattleBehaviour)
        {
            unitBattleBehaviour.Unit.ModifyHealth(4);
        }

        private void UnShieldUnit(UnitBattleBehaviour unitBattleBehaviour)
        {
            unitBattleBehaviour.Unit.ModifyHealth(-4);
        }
    }
}