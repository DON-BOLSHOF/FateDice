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
        public IObservable<UniRx.Unit> OnLevelUpped => _onLevelUpped;

        private ISpecializationProvider _specializationProvider;

        private ReactiveProperty<int> _xp { get; } = new();
        private ReactiveProperty<bool> _onReadyToLevelUp = new();
        private ReactiveCommand _onLevelUpped = new();

        private int _xpThreashold = 200;

        private CompositeDisposable _classDisposable = new();

        public Class(Specialization baseSpecialization)
        {
            _specializationProvider = new BaseSpecialization(baseSpecialization);

            _xp.Subscribe(_ => _onReadyToLevelUp.Value = _xp.Value >= _xpThreashold).AddTo(_classDisposable);
        }

        public void LevelUp(Specialization specialization)
        {
            if (_xp.Value < _xpThreashold)
                throw new ApplicationException("There are not enough xp to LevelUp");

            _specializationProvider = new SpecializationDecorator(_specializationProvider, specialization);

            _xp.Value -= _xpThreashold;
            _xpThreashold *= 2;

            _onLevelUpped?.Execute();
        }

        public void ModifyXP(int value)
        {
            _xp.Value += value;

            if (_xp.Value >= _xpThreashold)
                _onReadyToLevelUp.Value = true;
        }

        public List<Specialization> GetDecoratedSpecializations()
        {
            return _specializationProvider.GetProvidedSpecializations();
        }

        public void Dispose()
        {
            _onReadyToLevelUp?.Dispose();
            _onLevelUpped?.Dispose();
            _classDisposable?.Dispose();
            _xp?.Dispose();
        }
    }
}