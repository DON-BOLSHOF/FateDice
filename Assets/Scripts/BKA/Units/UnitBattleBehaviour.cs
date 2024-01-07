using BKA.Dices;
using BKA.Dices.Attributes;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public class UnitBattleBehaviour
    {
        private Unit _unit;

        private DiceObject _dice;

        public ReactiveCommand OnDead;

        private CompositeDisposable _disposable = new();
        
        public UnitBattleBehaviour(Unit unit, DiceObject dice)
        {
            _unit = unit;
            _dice = dice;

            _unit.Health.Where(value => value <= 0).Subscribe(_ => OnDead?.Execute()).AddTo(_disposable);
            _dice.OnDiceSelected.Subscribe(PrepareToAct).AddTo(_disposable);
        }

        private void PrepareToAct(DiceAction action)
        {
            Debug.Log(action.ID);
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