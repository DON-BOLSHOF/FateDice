using System.Collections.Generic;
using System.Linq;
using BKA.Buffs.Model;

namespace BKA.Buffs
{
    public class SpecializationDecorator : ISpecializationProvider
    {
        protected ISpecializationProvider _wrappedEntity;
        private Specialization _specialization;

        public SpecializationDecorator(ISpecializationProvider wrappedEntity, Specialization specialization)
        {
            _specialization = specialization;
            _wrappedEntity = wrappedEntity;
        }

        public IBuff GetBuff()
        {
            return GetSpecializationInternal();
        }

        public List<Specialization> GetProvidedSpecializations()
        {
            var specializations = _wrappedEntity.GetProvidedSpecializations();
            specializations.Add(_specialization);

            return specializations;
        }

        private IBuff GetSpecializationInternal()
        {
            var specialization = _wrappedEntity.GetBuff();
            
            var instance = new CommonBuff(_specialization.StatusOfBuff | specialization.StatusOfBuff, _specialization.DiceActionPairs,
                _specialization.Characteristics);

            if ((specialization.StatusOfBuff & BuffStatus.Characteristics) != 0)
            {
                instance.Characteristics.ModifyCharacteristics(specialization.Characteristics);
            }

            if ((specialization.StatusOfBuff & BuffStatus.Actions) != 0)
            {
                foreach (var diceActionPair in from diceActionPair in specialization.DiceActionPairs
                         let index = _specialization.DiceActionPairs.FindIndex(pair =>
                             pair.Index == diceActionPair.Index)
                         where index <= 0
                         select diceActionPair)
                {
                    instance.DiceActionPairs.Add(diceActionPair);
                }
            }

            return instance;
        }
    }
}