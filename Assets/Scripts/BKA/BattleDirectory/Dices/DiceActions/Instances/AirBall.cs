﻿using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/AirBall", menuName = "Defs/DiceAttribute/AirBall")]
    public class AirBall : DiceActionData
    {
        [field: SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field: SerializeField] public override Sprite ActionView { get; protected set; }
        [field: SerializeField] public override int BaseActValue { get; protected set; }

        public override DiceActionMainAttribute DiceActionMainAttribute { get; protected set; } =
            DiceActionMainAttribute.Intelligent;
        public override Action<UnitBattleBehaviour, ActionModificator> Act => Fire;
        public override Action<UnitBattleBehaviour, ActionModificator> Undo => UndoFire;

        private void Fire(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(-(BaseActValue + modificator.GetModificatorValue()));
        }

        private void UndoFire(UnitBattleBehaviour battleBehaviour, ActionModificator modificator)
        {
            battleBehaviour.Unit.ModifyHealth(BaseActValue +modificator.GetModificatorValue());
        }
    }
}