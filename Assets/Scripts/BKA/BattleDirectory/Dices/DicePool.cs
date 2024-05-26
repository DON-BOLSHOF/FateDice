using System.Collections.Generic;
using Zenject;

namespace BKA.Dices
{
    public class DicePool
    {
        [Inject] private DiceFactory _diceFactory;

        private Stack<CubeDice> _pool = new();

        public bool GetCubeDice(out CubeDice cubeDice)
        {
            cubeDice = _pool.Count > 0 ? _pool.Pop() : null;

            return cubeDice != null;
        }

        public void ReplenishDice(DiceObject dice)
        {
            if (dice is CubeDice cubeDice)
                _pool.Push(cubeDice);
        }
    }
}