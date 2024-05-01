using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/InActivity", menuName = "Defs/DiceAttribute/InActivity")]
    public class InActivity : DiceActionData
    {
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        [field: SerializeField] public override int BaseActValue { get; protected set; }
        
        public override DiceActionMainAttribute DiceActionMainAttribute { get; protected set; } =
            DiceActionMainAttribute.None;
        public override Action<UnitBattleBehaviour, ActionModificator> Act => DoNothing;
        public override Action<UnitBattleBehaviour, ActionModificator> Undo => DoNothing;

        private void DoNothing(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)//Заменить на модификатор
        {
        }
    }
}