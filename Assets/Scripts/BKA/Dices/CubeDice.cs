using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Dices
{
    public class CubeDice : DiceObject
    {
        protected override int FixedAttributesValue { get; } = 6;
        public override Rigidbody Rigidbody { get; protected set; }
        public override DiceAttribute[] DiceAttributes { get; protected set; }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
    }
}