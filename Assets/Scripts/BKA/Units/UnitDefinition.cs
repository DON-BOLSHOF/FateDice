using BKA.Dices.Attributes;
using UnityEngine;

namespace BKA.Units
{
    [CreateAssetMenu(menuName = "Additional/UnitDefinition", fileName = "UnitDefinition")]
    public class UnitDefinition : ScriptableObject
    {
        [field:SerializeField] public string ID { get; private set; }
        [field:SerializeField] public DiceAction[] DiceActions { get; private set; }
        [field:SerializeField] public int FixedActionsValue { get; private set; }
        
        [field:SerializeField] public int Health { get; private set; }
        
        [field:SerializeField] public Sprite UnitIcon { get; private set; }
    }
}