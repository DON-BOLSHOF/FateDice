using System;
using BKA.Buffs;
using BKA.Dices.DiceActions;
using UniRx;

namespace BKA.Units
{
    public class Rat : Unit
    {
        public override UnitDefinition Definition { get; protected set; }
        public override DiceActionData[] DiceActions { get; protected set; }
        public override Class Class { get; }
        protected override int _maximumHealth { get; set; }
        protected sealed override ReactiveProperty<int> _health { get; } = new();

        public Rat(DefinitionPool definitionPool)
        {
            Definition = definitionPool.GetFromPool("Крыса");

            DiceActions = new DiceActionData[Definition.BaseDiceActions.Length];
            Array.Copy(Definition.BaseDiceActions, DiceActions, Definition.BaseDiceActions.Length);
            
            _health.Value = Definition.BaseHealth;
            _maximumHealth = Definition.BaseHealth;
            
            Class = new Class(new Specialization(Definition.BaseSpecializationDefinition), Definition.BaseCharacteristics.Clone());
            Class.OnClassModified?.Subscribe(_ => UpdateData()).AddTo(_unitDisposable);
          
            UpdateData();
        }
    }
}