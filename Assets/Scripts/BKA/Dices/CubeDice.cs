using System;
using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Dices
{
    public class CubeDice : DiceObject
    {
        [field: SerializeField] protected override DiceEdge[] _diceEdges { get; set; }
        protected override int FixedAttributesValue => 6;
        public override Rigidbody Rigidbody { get; protected set; }
        public override DiceAction[] DiceAttributes { get; protected set; }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                foreach (var diceEdge in _diceEdges)
                {
                    if (diceEdge.CheckNotCrossEnvironment())
                        Debug.Log(diceEdge.name);
                }
            }
        }
    }
}