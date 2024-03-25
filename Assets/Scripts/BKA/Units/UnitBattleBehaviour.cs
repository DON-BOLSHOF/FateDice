using System;
using System.Linq;
using BKA.Dices;
using BKA.Dices.DiceActions;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BKA.Units
{
    public class UnitBattleBehaviour : IUnitOfBattle, IDisposable
    {
        public Unit Unit { get; }
        public DiceObject DiceObject { get; }
        public ReactiveCommand OnDead { get; } = new();
        public DiceAction DiceAction => _diceAction;
        
        private CompositeDisposable _disposable = new();

        private DiceAction _diceAction;
        
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
        }

        public async UniTask Act()
        {
            Debug.Log("Act");
            
            DiceAction.Act();

            await UniTask.Delay(TimeSpan.FromSeconds(5));
        }
        
        public void UndoAct()
        {
            Debug.Log("Undo");
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            OnDead?.Dispose();
        }
    }
}