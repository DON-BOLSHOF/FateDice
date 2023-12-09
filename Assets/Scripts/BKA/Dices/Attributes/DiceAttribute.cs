using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute", menuName = "Additional/DiceAttribute")]
    public abstract class DiceAttribute : ScriptableObject
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