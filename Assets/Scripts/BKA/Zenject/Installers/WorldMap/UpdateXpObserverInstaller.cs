using BKA.WorldMapDirectory;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/UpdateXpObserverInstaller", fileName = "UpdateXpObserverInstaller")]
    public class UpdateXpObserverInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UpdateXPObserver>().AsSingle().NonLazy();
        }
    }
}