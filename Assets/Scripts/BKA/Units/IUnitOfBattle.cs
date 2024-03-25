using BKA.Dices;
using BKA.Dices.DiceActions;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public interface IUnitOfBattle
    {
        public Unit Unit { get; }
        public ReactiveCommand OnDead { get; }
        public DiceObject DiceObject { get; }
        public DiceAction DiceAction { get; }
    }
}