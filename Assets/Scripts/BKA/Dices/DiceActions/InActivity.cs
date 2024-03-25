using System;
using BKA.Units;
using UnityEngine;

namespace BKA.Dices.DiceActions
{
    [CreateAssetMenu(fileName = "DiceAttribute/InActivity", menuName = "Additional/DiceAttribute/InActivity")]
    public class InActivity : DiceActionData
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttributeFocus DiceAttributeFocus { get; protected set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
        public override Action<UnitBattleBehaviour> Action => DoNothing;

        private void DoNothing(UnitBattleBehaviour battleBehaviour)
        {
        }
    }
}