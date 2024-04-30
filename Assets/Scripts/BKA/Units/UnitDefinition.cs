using BKA.Buffs;
using BKA.Dices.DiceActions;
using UnityEngine;

namespace BKA.Units
{
    [CreateAssetMenu(menuName = "Defs/UnitDefinition", fileName = "UnitDefinition")]
    public class UnitDefinition : ScriptableObject
    {
        [field:SerializeField] public string ID { get; private set; }
        [field:SerializeField] public string UnitDescription { get; private set; }
        [field:SerializeField] public Sprite UnitIcon { get; private set; }
        
        [field:SerializeField] public DiceActionData[] DiceActions { get; private set; }
        [field:SerializeField] public int FixedActionsValue { get; private set; }
        
        [field:SerializeField] public int Health { get; private set; }

        [field: SerializeField] public SpecializationDefinition BaseSpecializationDefinition { get; private set; } 
    }
}