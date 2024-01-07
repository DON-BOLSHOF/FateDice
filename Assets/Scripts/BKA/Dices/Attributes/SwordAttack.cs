using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute/SwordAttack", menuName = "Additional/DiceAttribute/SwordAttack")]
    public class SwordAttack : DiceAction
    { 
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] protected override DiceAttributeFocus _diceAttributeFocus { get; set; }
        [field:SerializeField] public override Sprite ActionView { get; protected set; }
    }
}