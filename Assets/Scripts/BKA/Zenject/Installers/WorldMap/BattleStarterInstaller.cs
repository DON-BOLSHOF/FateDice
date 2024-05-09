using BKA.WorldMapDirectory.Systems;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/BattleStarterInstaller", fileName = "BattleStarterInstaller")]
    public class BattleStarterInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattleStarter>().AsSingle();
        }
    }
}