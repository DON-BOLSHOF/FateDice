using BKA.Dices;
using BKA.Dices.Attributes;
using BKA.UI;
using BKA.Utils;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public class UnitBattleBehaviour : IUnitOfBattle
    {
        public Unit Unit { get; }
        public ReactiveCommand OnDead { get; } = new();

        public DiceObject DiceObject { get; }
        
        private CompositeDisposable _disposable = new();
        
        public UnitBattleBehaviour(Unit unit, DiceObject dice)
        {
            Unit = unit;
            DiceObject = dice;

            Unit.Health.Where(value => value <= 0).Subscribe(_ => OnDead?.Execute()).AddTo(_disposable);
            DiceObject.OnDiceSelected.Subscribe(PrepareToAct).AddTo(_disposable);
        }

        private void PrepareToAct(DiceAction action)
        {
            Debug.Log(action.ID);
        }
        
        private void UndoAct()
        {
            Debug.Log("Undo");
        }

        ~UnitBattleBehaviour()
        {
            _disposable.Dispose();
            _disposable.Clear();

            _disposable = null;
        }
    }

    public enum UnitBattleState
    {
        NotReady,
        Ready
    }
}