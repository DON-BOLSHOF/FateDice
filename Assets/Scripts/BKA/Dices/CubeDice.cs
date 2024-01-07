using System;
using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Dices
{
    public class CubeDice : DiceObject
    {
        [field: SerializeField] protected override DiceEdge[] _diceEdges { get; set; }
        protected override int FixedActionsAmount => 6;
        public override Rigidbody Rigidbody { get; protected set; }
        public override DiceAction[] DiceActions { get; protected set; }
    }
}