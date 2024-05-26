using System.Collections.Generic;
using BKA.Dices.DiceActions;
using BKA.Units;

namespace BKA.Buffs.Model
{
    public class CommonBuff : IBuff
    {
        public BuffStatus StatusOfBuff { get; }
        public List<DiceActionPair> DiceActionPairs { get; }
        public Characteristics Characteristics { get; }

        public CommonBuff(BuffStatus buffStatus, List<DiceActionPair> diceActionPairs, Characteristics characteristics)
        {
            StatusOfBuff = buffStatus;
            DiceActionPairs = new List<DiceActionPair>(diceActionPairs);
            Characteristics = new Characteristics(characteristics.Agility, characteristics.Strength, characteristics.Intelligent);
        }
    }
}