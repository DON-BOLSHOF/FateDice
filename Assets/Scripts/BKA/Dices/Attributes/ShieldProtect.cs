using UnityEngine;

namespace BKA.Dices.Attributes
{
    [CreateAssetMenu(fileName = "DiceAttribute/ShieldProtect", menuName = "Additional/DiceAttribute/ShieldProtect")]
    public class ShieldProtect : DiceAction
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] protected override DiceAttributeFocus _diceAttributeFocus { get; set; }
    }
}