using System;
using System.Collections.Generic;
using BKA.Buffs;
using UniRx;

namespace BKA.Units
{
    public class Class : IDisposable
    {
        public IBuff ClassBuff => _specializationProvider.GetBuff();

        public float XPPercentage => _xp.Value / (float)_xpThreashold;
        public IReadOnlyReactiveProperty<bool> OnReadyToLevelUp => _onReadyToLevelUp;
        public IObservable<UniRx.Unit> OnClassModified => _onClassModified;
        public Characteristics Characteristics => _characteristics;

        private ISpecializationProvider _specializationProvider;
        private Characteristics _characteristics;

        private ReactiveProperty<int> _xp { get; } = new();
        private ReactiveProperty<bool> _onReadyToLevelUp = new();
        private ReactiveCommand _onClassModified = new();

        private int _xpThreashold = 200;
        
        private CompositeDisposable _classDisposable = new();

        public Class(Specialization baseSpecialization, Characteristics characteristics)
        {
            _specializationProvider = new BaseSpecialization(baseSpecialization);
            _characteristics = characteristics;

            _xp.Subscribe(_ => _onReadyToLevelUp.Value = _xp.Value >= _xpThreashold).AddTo(_classDisposable);
        }

        public void LevelUp(Specialization specialization)
        {
            if (_xp.Value < _xpThreashold)
                throw new ApplicationException("There are not enough xp to LevelUp");

            _specializationProvider = new SpecializationDecorator(_specializationProvider, specialization);

            _xp.Value -= _xpThreashold;
            _xpThreashold *= 2;

            _onClassModified?.Execute();
        }

        public void ModifyXP(int value)
        {
            _xp.Value += value;

            if (_xp.Value >= _xpThreashold)
                _onReadyToLevelUp.Value = true;

            _onClassModified?.Execute();
        }

        public List<Specialization> GetDecoratedSpecializations()
        {
            return _specializationProvider.GetProvidedSpecializations();
        }

        public void Dispose()
        {
            _onReadyToLevelUp?.Dispose();
            _onClassModified?.Dispose();
            _classDisposable?.Dispose();
            _xp?.Dispose();
        }
    }
}