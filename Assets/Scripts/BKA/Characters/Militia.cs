using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Characters
{
    [CreateAssetMenu(fileName = "Character/Militia", menuName = "Additional/Character/Militia")]
    public class Militia : Character
    {
        [field:SerializeField] public override string ID { get; protected set; }
        [field:SerializeField] public override DiceAttribute[] DiceAttributes { get; protected set; }
        protected override int FixedAttributesValue { get; } = 6;
    }
}