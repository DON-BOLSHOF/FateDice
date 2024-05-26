using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/WolfRoar", menuName = "Defs/DiceAttribute/WolfRoar")]
    public class WolfRoar : DiceActionData
    {
        [field: SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field: SerializeField] public override Sprite ActionView { get; protected set; }
        [field: SerializeField] public override int BaseActValue { get; protected set; }

        public override DiceActionMainAttribute DiceActionMainAttribute { get; protected set; } =
            DiceActionMainAttribute.Agility;

        public override Action<UnitBattleBehaviour, ActionModificator> Act => Attack;
        public override Action<UnitBattleBehaviour, ActionModificator> Undo => UndoAttack;

        private void Attack(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(-(BaseActValue + modificator.GetModificatorValue()));
        }

        private void UndoAttack(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth((BaseActValue + modificator.GetModificatorValue()));
        }
    }
}