using System.Collections.Generic;
using BKA.Dices.DiceActions;
using UnityEngine;

namespace BKA.Buffs
{
    public interface IBuff
    {
        public BuffStatus BuffStatus { get; }
        public List<DiceActionPair> DiceActionPairs { get; }
    }
}