using System.Collections.Generic;

namespace BKA.Buffs
{
    public interface ISpecializationProvider
    {
        IBuff GetBuff();

        List<Specialization> GetProvidedSpecializations();
    } 
}