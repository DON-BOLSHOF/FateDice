using UnityEngine;

namespace BKA.Dices.Attributes
{
    public abstract class DiceAction : ScriptableObject
    {
        public abstract string ID { get; protected set; }
        protected abstract DiceAttributeFocus _diceAttributeFocus { get; set;}
        public abstract Sprite ActionView { get; protected set; }
    }

    public enum DiceAttributeFocus
    {
        None,
        Enemy,
        Ally
    }
}