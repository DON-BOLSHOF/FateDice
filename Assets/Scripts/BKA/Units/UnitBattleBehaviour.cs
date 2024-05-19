﻿using System;
using System.Linq;
using BKA.Dices;
using BKA.Dices.DiceActions;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public class UnitBattleBehaviour : IUnitOfBattle, IDisposable
    {
        public Unit Unit { get; }
        public DiceObject DiceObject { get; }
        public ReactiveCommand OnDead { get; } = new();
        public ReactiveCommand OnDamaged { get; } = new();
        public ReactiveCommand OnHealed { get; } = new();
        public DiceAction DiceAction => _diceAction;

        private DiceAction _diceAction;

        private CompositeDisposable _disposable = new();

        public readonly ReactiveProperty<bool> IsReadyToAct = new(false);
        public readonly ReactiveProperty<bool> IsActed = new(false);

        public UnitBattleBehaviour(Unit unit, DiceObject dice)
        {
            Unit = unit;
            DiceObject = dice;

            Unit.Health.Where(value => value <= 0).Subscribe(_ => OnDead?.Execute()).AddTo(_disposable);

            var localHealth = Unit.Health.Value;
            Unit.Health.Subscribe(newValue =>
            {
                if (newValue < localHealth)
                {
                    OnDamaged?.Execute();
                    Debug.Log("Damaged");
                }
                
                if (newValue > localHealth)
                {
                    OnHealed?.Execute();
                    Debug.Log("Healed");
                }
                
                localHealth = newValue;
            }).AddTo(_disposable);

            dice.UpdateActions(unit.DiceActions.Select(data =>
                new DiceAction(data,
                    new CharacteristicActionProvider(unit.Class.Characteristics, data.DiceActionMainAttribute)
                        .GetModificator())).ToArray());

            DiceObject.OnDiceReadyToAct.Subscribe(PrepareToAct).AddTo(_disposable);
            DiceObject.OnDiceUnReadyToAct.Subscribe(_ => UnPrepareToAct()).AddTo(_disposable);
        }

        private void PrepareToAct(DiceAction actionData)
        {
            _diceAction = actionData;
            IsReadyToAct.Value = true;
            IsActed.Value = false;
        }

        public void UnPrepareToAct()
        {
            _diceAction = null;
            IsReadyToAct.Value = false;
        }

        public void Act()
        {
            _diceAction.Act();

            IsActed.Value = true;
            IsReadyToAct.Value = false;
        }

        public void UndoAct()
        {
            _diceAction.Undo();

            IsActed.Value = false;
            IsReadyToAct.Value = true;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            OnDead?.Dispose();
        }
    }
}