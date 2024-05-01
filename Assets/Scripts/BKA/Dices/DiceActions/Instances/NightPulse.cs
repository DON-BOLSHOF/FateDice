using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/NightPulse", menuName = "Defs/DiceAttribute/NightPulse")]
    public class NightPulse : DiceActionData
    {
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        [field: SerializeField] public override int BaseActValue { get; protected set; }
        
        public override DiceActionMainAttribute DiceActionMainAttribute { get; protected set; } =
            DiceActionMainAttribute.Intelligent;
        public override Action<UnitBattleBehaviour, ActionModificator> Act => Pulse;
        public override Action<UnitBattleBehaviour, ActionModificator> Undo => UndoPulse;

        private void Pulse(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(-(BaseActValue +  modificator.GetModificatorValue()));
        }
        
        private void UndoPulse(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(BaseActValue +  modificator.GetModificatorValue());
        }
    }
}