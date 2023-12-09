using System;
using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Characters
{
    [CreateAssetMenu(menuName = "Additional/Character", fileName = "Character")]
    public abstract class Character : ScriptableObject
    {
        public abstract string ID { get; protected set; }
        public abstract DiceAttribute[] DiceAttributes { get; protected set; }
        protected abstract int FixedAttributesValue { get; }

        private void Awake()
        {
            if (DiceAttributes.Length != FixedAttributesValue)
            {
                throw new ArgumentException("Attributes does not equal to fixedValue");
            }
        }
    }
}