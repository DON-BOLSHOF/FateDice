using System.Collections.Generic;
using BKA.Units;
using Cysharp.Threading.Tasks;

namespace BKA.WorldMapDirectory.Systems.Interfaces
{
    public interface IBattleStarter
    {
        UniTaskVoid StartBattle(Unit[] enemies, int xpValue);
    }
}