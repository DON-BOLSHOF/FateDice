using System.Collections.Generic;
using BKA.Units;

namespace BKA.Zenject.Signals
{
    public class ExtraordinaryBattleSignal : Signal
    {
        public IEnumerable<UnitDefinition> Enemies;
        public int Xp;
    }
}