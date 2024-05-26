using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;
using UnityEngine;

namespace BKA.Buffs
{
    public interface IBuff
    {
        public BuffStatus StatusOfBuff { get; }
        public List<DiceActionPair> DiceActionPairs { get; }
        public Characteristics Characteristics { get; }
    }
}