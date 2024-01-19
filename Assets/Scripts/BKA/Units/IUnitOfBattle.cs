using BKA.Dices;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public interface IUnitOfBattle
    {
        public Unit Unit { get; }
        public ReactiveCommand OnDead { get; }
        public DiceObject DiceObject { get; }
    }
}