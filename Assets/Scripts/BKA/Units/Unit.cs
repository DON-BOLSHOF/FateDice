using System;
using System.Collections.Generic;
using System.Linq;
using BKA.Dices.DiceActions;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;

namespace BKA.Units
{
    public abstract class Unit
    {
        public abstract UnitDefinition Definition { get; protected set; }

        public abstract DiceActionData[] DiceActions { get; protected set; }
        public IReadOnlyReactiveProperty<int> Health => _health;

        public List<Artefact> Artefacts { get; protected set; } = new();

        protected abstract ReactiveProperty<int> _health { get; }

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

        private void UpdateData()
        {
            DiceActions = new DiceActionData[Definition.DiceActions.Length];
            Array.Copy(Definition.DiceActions, DiceActions, Definition.DiceActions.Length);
            
            foreach (var artefactDiceActionPair in 
                     Artefacts.Where(artefact => (artefact.ArtefactStatus & ArtefactStatus.Actions) != 0)
                         .SelectMany(artefact => artefact.DiceActionPairs))
            {
                ModifyAction(artefactDiceActionPair.DiceAction, artefactDiceActionPair.Index);
            }
        }
    }
}