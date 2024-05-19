﻿using System;
using BKA.Buffs;
using BKA.Dices.DiceActions;
using UniRx;

namespace BKA.Units
{
    public class HellMage : Unit
    {
        public sealed override UnitDefinition Definition { get; protected set; }
        public sealed override DiceActionData[] DiceActions { get; protected set; }
        public sealed override Class Class { get; }
        protected sealed override int _maximumHealth { get; set; }
        protected sealed override ReactiveProperty<int> _health { get; } = new();

        public HellMage(DefinitionPool definitionPool)
        {
            Definition = definitionPool.GetFromPool("Эдара");
            
            DiceActions = new DiceActionData[Definition.BaseDiceActions.Length];
            Array.Copy(Definition.BaseDiceActions, DiceActions, Definition.BaseDiceActions.Length);
            
            _maximumHealth = Definition.BaseHealth;
            _health.Value = Definition.BaseHealth;
            
            Class = new Class(new Specialization(Definition.BaseSpecializationDefinition), Definition.BaseCharacteristics.Clone());
            Class.OnClassModified?.Subscribe(_ => UpdateData()).AddTo(_unitDisposable);
          
            UpdateData();
        }
    }
}