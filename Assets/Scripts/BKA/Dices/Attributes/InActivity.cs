using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute/InActivity", menuName = "Additional/DiceAttribute/InActivity")]
    public class InActivity : DiceAction
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] protected override DiceAttributeFocus _diceAttributeFocus { get; set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
    }
}