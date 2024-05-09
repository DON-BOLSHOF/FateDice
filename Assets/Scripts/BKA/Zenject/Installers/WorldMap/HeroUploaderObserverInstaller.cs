using BKA.System;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/HeroUploaderObserverInstaller", fileName = "HeroUploaderObserverInstaller")]
    public class HeroUploaderObserverInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HeroUploaderObserver>().AsSingle();
        }
    }
}