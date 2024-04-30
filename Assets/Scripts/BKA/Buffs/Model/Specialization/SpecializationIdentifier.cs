using System.Collections.Generic;

namespace BKA.Buffs
{
    public class SpecializationIdentifier
    {
        private List<SpecializationSequence> _specializationSequences;

        public SpecializationIdentifier(List<SpecializationSequence> sequences)
        {
            _specializationSequences = sequences;
        }

        public SpecializationSequence IdentifySequence(Specialization specialization)
        {
            return _specializationSequences.Find(sequence =>
                sequence.CurrentSpecialization.Equals(specialization.Definition));
        }
    }
}