using BKA.WorldMapDirectory.Systems;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class BattlePointsInstaller : MonoInstaller
    {
        [SerializeField] private BattlePoint[] _battlePoints;
        
        public override void InstallBindings()
        {
            foreach (var battlePoint in _battlePoints)
            {
                Container.Bind<BattlePoint>().FromInstance(battlePoint).AsCached();
            }
        }
    }
}