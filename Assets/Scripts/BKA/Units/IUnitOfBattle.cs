using BKA.Dices;
using UniRx;
using UnityEngine;

namespace BKA.Units
{
    public interface IUnitOfBattle
    {
        public ReactiveCommand OnDead { get; }
        public Vector3 Position { get; }
        public DiceObject DiceObject { get; }
    }
}