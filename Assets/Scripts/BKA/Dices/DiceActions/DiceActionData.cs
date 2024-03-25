using System;
using BKA.Units;
using Unity.VisualScripting;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    public abstract class DiceActionData : ScriptableObject
    {
        public abstract string ID { get; protected set; }
        public abstract DiceAttributeFocus DiceAttributeFocus { get; protected set;}
        public abstract Sprite ActionView { get; protected set; }

        public abstract Action<UnitBattleBehaviour> Action { get; }
    }

    public enum DiceAttributeFocus
    {
        None,
        Enemy,
        Ally
    }
}