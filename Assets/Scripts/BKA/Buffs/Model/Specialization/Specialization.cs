using System.Collections.Generic;
using BKA.Dices.DiceActions;

namespace BKA.Buffs
{
    public class Specialization : IBuff
    {
        public BuffStatus BuffStatus { get; }
        public List<DiceActionPair> DiceActionPairs { get; }
        
        public SpecializationDefinition Definition { get; }

        public Specialization(SpecializationDefinition specializationDefinition)
        {
            Definition = specializationDefinition;
            
            BuffStatus = specializationDefinition.BuffStatus;
            DiceActionPairs = specializationDefinition.DiceAction;
        }
    }
}