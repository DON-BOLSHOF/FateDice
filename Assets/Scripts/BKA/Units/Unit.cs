using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Buffs;
using BKA.Dices.DiceActions;
using UniRx;

namespace BKA.Units
{
    public abstract class Unit : IDisposable
    {
        public abstract UnitDefinition Definition { get; protected set; }
        public abstract DiceActionData[] DiceActions { get; protected set; }
        public abstract Class Class { get; }
        
        public IReadOnlyReactiveProperty<int> Health => _health;

        public List<Artefact> Artefacts { get; protected set; } = new();

        public IObservable<UniRx.Unit> OnUpdatedData => _onUpdatedData;

        protected abstract int _maximumHealth { get; set; }
        protected abstract ReactiveProperty<int> _health { get; }

        protected ReactiveCommand _onUpdatedData { get; } = new();
        protected CompositeDisposable _unitDisposable { get; } = new();

        public void ModifyHealth(int value)
        {
            _health.Value = Math.Clamp(_health.Value + value, 0, _maximumHealth);
        }

        public void SetArtefacts(List<Artefact> artefacts)
        {
            Artefacts = artefacts;
            UpdateData();
        }

        public void Dispose()
        {
            _unitDisposable?.Dispose();
            _health?.Dispose();
            Class?.Dispose();
        }

        protected void UpdateData()
        {
            DiceActions = new DiceActionData[Definition.BaseDiceActions.Length];
            Array.Copy(Definition.BaseDiceActions, DiceActions, Definition.BaseDiceActions.Length);

            var specializationBuffs = Class.ClassBuff;
            var characteristics = Definition.BaseCharacteristics.Clone();

            characteristics.ModifyCharacteristics(specializationBuffs.Characteristics);

            foreach (var characteristic in
                     Artefacts.Where(artefact => (artefact.StatusOfBuff & BuffStatus.Characteristics) != 0)
                         .Select(artefact => artefact.Characteristics))
            {
                characteristics.ModifyCharacteristics(characteristic);
            }

            Class.Characteristics.FullUpdateData(characteristics);

            var localHpPercentage = (float)_health.Value / _maximumHealth;
            _maximumHealth = Definition.BaseHealth + characteristics.Strength / 2;
            _health.Value = (int)(localHpPercentage * _maximumHealth);
            
            if ((specializationBuffs.StatusOfBuff & BuffStatus.Actions) != 0)
            {
                foreach (var specializationBuffsDiceActionPair in specializationBuffs.DiceActionPairs)
                {
                    DiceActions[specializationBuffsDiceActionPair.Index] = specializationBuffsDiceActionPair.DiceAction;
                }
            }

            foreach (var artefactDiceActionPair in
                     Artefacts.Where(artefact => (artefact.StatusOfBuff & BuffStatus.Actions) != 0)
                         .SelectMany(artefact => artefact.DiceActionPairs))
            {
                DiceActions[artefactDiceActionPair.Index] = artefactDiceActionPair.DiceAction;
            }

            _onUpdatedData?.Execute();
        }
    }
}