using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;

namespace BKA.Buffs
{
    public class Specialization : IBuff
    {
        public BuffStatus StatusOfBuff { get; }
        public List<DiceActionPair> DiceActionPairs { get; }
        public Characteristics Characteristics { get; }

        public SpecializationDefinition Definition { get; }

        public Specialization(SpecializationDefinition specializationDefinition)
        {
            Definition = specializationDefinition;
            
            StatusOfBuff = specializationDefinition.StatusOfBuff;
            DiceActionPairs = specializationDefinition.DiceAction;
            Characteristics = specializationDefinition.Characteristics;
        }
    }
}