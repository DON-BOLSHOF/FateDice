using System;
using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Dices
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class DiceObject : MonoBehaviour
    {
        public abstract Rigidbody Rigidbody { get; protected set; }
        public abstract DiceAttribute[] DiceAttributes { get; protected set; }
        protected abstract int FixedAttributesValue { get; }
        protected abstract DiceEdge[] _diceEdges { get; set;}

        public void ModifyAttributes(DiceAttribute[] diceAttributes)
        {
            if (diceAttributes.Length != FixedAttributesValue)
            {
                throw new ArgumentException();
            }

            DiceAttributes = diceAttributes;
        }
    }
}