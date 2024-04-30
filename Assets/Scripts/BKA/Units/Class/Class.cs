using System;
using System.Collections.Generic;
using BKA.Buffs;
using UniRx;

namespace BKA.Units
{
    public class Class
    {
        private ISpecializationProvider _specializationProvider;
        public IBuff SpecializationDecoratedBuff => _specializationProvider.GetBuff();
        public IObservable<UniRx.Unit> OnDecorated => _onDecorated;

        private ReactiveCommand _onDecorated = new();
        
        public Class(Specialization baseSpecialization)
        {
            _specializationProvider = new BaseSpecialization(baseSpecialization);
        }

        public void DecorateSpecialization(Specialization specialization)
        {
            _specializationProvider = new SpecializationDecorator(_specializationProvider, specialization);
            _onDecorated?.Execute();
        }

        public List<Specialization> GetDecoratedSpecializations()
        {
            return _specializationProvider.GetProvidedSpecializations();
        }
    }
}