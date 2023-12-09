using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute/SwordAttack", menuName = "Additional/DiceAttribute/SwordAttack")]
    public class SwordAttack : DiceAttribute
    { 
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] protected override DiceAttributeFocus _diceAttributeFocus { get; set; }
    }
}