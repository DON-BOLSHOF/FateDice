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
        
        public IReadOnlyReactiveProperty<int> Health => _health;
        public List<Artefact> Artefacts { get; protected set; } = new();
        public abstract Class Class { get; }

        public IObservable<UniRx.Unit> OnUpdatedData => _onUpdatedData;
        
        protected ReactiveProperty<int> _health { get; } = new();
        protected ReactiveCommand _onUpdatedData { get; } = new();
        protected readonly CompositeDisposable _unitDisposable = new();

        public void ModifyHealth(int value)
        {
            _health.Value += value;
        }

        public void ModifyAction(DiceActionData data, int index)
        {
            DiceActions[index] = data;
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
        }

        protected void UpdateData()
        {
            DiceActions = new DiceActionData[Definition.DiceActions.Length];
            Array.Copy(Definition.DiceActions, DiceActions, Definition.DiceActions.Length);

            var specializationBuffs = Class.SpecializationDecoratedBuff;
            if ((specializationBuffs.BuffStatus & BuffStatus.Actions) != 0)
            {
                foreach (var specializationBuffsDiceActionPair in specializationBuffs.DiceActionPairs)
                {
                    ModifyAction(specializationBuffsDiceActionPair.DiceAction, specializationBuffsDiceActionPair.Index);
                }
            }

            foreach (var artefactDiceActionPair in
                     Artefacts.Where(artefact => (artefact.BuffStatus & BuffStatus.Actions) != 0)
                         .SelectMany(artefact => artefact.DiceActionPairs))
            {
                ModifyAction(artefactDiceActionPair.DiceAction, artefactDiceActionPair.Index);
            }

            _onUpdatedData?.Execute();
        }
    }
}