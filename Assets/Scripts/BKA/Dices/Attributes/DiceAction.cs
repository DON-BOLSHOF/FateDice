using UnityEngine;

namespace BKA.Dices.Attributes
{
    public abstract class DiceAction : ScriptableObject
    {
        public abstract string ID { get; protected set; }
        protected abstract DiceAttributeFocus _diceAttributeFocus { get; set;}
    }

    public enum DiceAttributeFocus
    {
        None,
        Enemy,
        Ally
    }
}