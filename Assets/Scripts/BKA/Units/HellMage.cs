using System;
using BKA.Dices.DiceActions;
using BKA.WorldMapDirectory.Artefacts;
using UniRx;

namespace BKA.Units
{
    public class HellMage : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }
        public sealed override DiceActionData[] DiceActions { get; protected set; }
        protected sealed override ReactiveProperty<int> _health { get; } = new();

        public HellMage(DefinitionPool definitionPool)
        {
            Definition = definitionPool.GetFromPool("HellMage");
            
            DiceActions = new DiceActionData[Definition.DiceActions.Length];
            Array.Copy(Definition.DiceActions, DiceActions, Definition.DiceActions.Length);
            
            _health.Value = Definition.Health;
        }
    }
}