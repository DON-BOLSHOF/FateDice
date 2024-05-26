using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    public abstract class DiceActionData : ScriptableObject
    {
        public abstract DiceAttributeFocus DiceAttributeFocus { get; protected set;}
        public abstract Sprite ActionView { get; protected set; }
        public abstract int BaseActValue { get; protected set; }
        public abstract DiceActionMainAttribute DiceActionMainAttribute { get; protected set; }

        public abstract Action<UnitBattleBehaviour, ActionModificator> Act { get; }
        public abstract Action<UnitBattleBehaviour, ActionModificator> Undo { get; }
    }

    public enum DiceAttributeFocus
    {
        None,
        Enemy,
        Ally
    }

    public enum DiceActionMainAttribute
    {
        None,
        Agility,
        Strength,
        Intelligent
    }
}