using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute/InActivity", menuName = "Additional/DiceAttribute/InActivity")]
    public class InActivity : DiceAttribute
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] protected override DiceAttributeFocus _diceAttributeFocus { get; set; }
    }
}