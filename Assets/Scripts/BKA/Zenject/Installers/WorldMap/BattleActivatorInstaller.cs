using BKA.WorldMapDirectory.Systems;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/BattleActivatorInstaller", fileName = "BattleActivatorInstaller")]
    public class BattleActivatorInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattleActivator>().AsSingle();
        }
    }
}