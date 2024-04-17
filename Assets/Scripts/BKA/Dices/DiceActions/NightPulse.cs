using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/NightPulse", menuName = "Additional/DiceAttribute/NightPulse")]
    public class NightPulse : DiceActionData
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        public override Action<UnitBattleBehaviour> Act => Pulse;
        public override Action<UnitBattleBehaviour> Undo => UndoPulse;

        private void Pulse(UnitBattleBehaviour battleBehaviour)
        {
            battleBehaviour.Unit.ModifyHealth(5);
        }
        
        private void UndoPulse(UnitBattleBehaviour battleBehaviour)
        {
            battleBehaviour.Unit.ModifyHealth(-5);
        }
    }
}