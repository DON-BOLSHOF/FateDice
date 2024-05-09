using BKA.WorldMapDirectory.Systems;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/BattleHandlerInstaller", fileName = "BattleHandlerInstaller")]
    public class BattleHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattleHandler>().AsSingle();
        }
    }
}