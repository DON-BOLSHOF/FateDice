using System.Collections.Generic;

namespace BKA.Buffs
{
    public class BaseSpecialization : ISpecializationProvider
    {
        private readonly Specialization _specialization;
        
        public BaseSpecialization(Specialization specialization)
        {
            _specialization = specialization;
        }

        public IBuff GetBuff()
        {
            return _specialization;
        }

        public List<Specialization> GetProvidedSpecializations()
        {
            return new List<Specialization> { _specialization };
        }
    }
}