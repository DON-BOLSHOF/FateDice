using System.Collections.Generic;
using BKA.Dices.DiceActions;

namespace BKA.Buffs.Model
{
    public class CommonBuff : IBuff
    {
        public BuffStatus BuffStatus { get; }
        public List<DiceActionPair> DiceActionPairs { get; }

        public CommonBuff(BuffStatus buffStatus, List<DiceActionPair> diceActionPairs)
        {
            BuffStatus = buffStatus;
            DiceActionPairs = diceActionPairs;
        }
    }
}