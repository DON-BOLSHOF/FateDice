using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/ShieldProtect", menuName = "Defs/DiceAttribute/ShieldProtect")]
    public class HealSpell : DiceActionData
    {
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        [field: SerializeField] public override int BaseActValue { get; protected set; }
        
        public override DiceActionMainAttribute DiceActionMainAttribute { get; protected set; } =
            DiceActionMainAttribute.Intelligent;
        
        public override Action<UnitBattleBehaviour, ActionModificator> Act => HealCast;
        public override Action<UnitBattleBehaviour, ActionModificator> Undo => UnHealCast;

        private void HealCast(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth((BaseActValue + modificator.GetModificatorValue()));
        }

        private void UnHealCast(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(-(BaseActValue + modificator.GetModificatorValue()));
        }
    }
}