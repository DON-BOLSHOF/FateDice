using System;
using System.Linq;
using System.Threading;
using BKA.Dices;
using BKA.Dices.DiceActions;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public class UnitBattleBehaviour : IUnitOfBattle, IDisposable
    {
        public Unit Unit { get; }
        public DiceObject DiceObject { get; }
        public ReactiveCommand OnDead { get; } = new();
        public DiceAction DiceAction => _diceAction;
        
        private DiceAction _diceAction;
        
        private CompositeDisposable _disposable = new();

        public readonly ReactiveProperty<bool> IsReadyToAct = new(false);

        public UnitBattleBehaviour(Unit unit, DiceObject dice)
        {
            Unit = unit;
            DiceObject = dice;

            Unit.Health.Where(value => value <= 0).Subscribe(_ => OnDead?.Execute()).AddTo(_disposable);
            
            dice.UpdateActions(unit.Definition.DiceActions.Select(data => new DiceAction(data)).ToArray());
            
            DiceObject.OnDiceSelected.Subscribe(PrepareToAct).AddTo(_disposable);
        }

        private void PrepareToAct(DiceAction actionData)
        {
            _diceAction= actionData;
            IsReadyToAct.Value = true;
        }

        public void UnPrepareToAct()
        {
            IsReadyToAct.Value = false;
        }

        public async UniTask Act(CancellationToken token)
        {
            DiceAction.Act();

            //await UniTask.Delay(TimeSpan.FromSeconds(5));

            IsReadyToAct.Value = false;
        }

        public void UndoAct()
        {
            IsReadyToAct.Value = true;
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            OnDead?.Dispose();
        }
    }
}