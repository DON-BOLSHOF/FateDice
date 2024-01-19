using BKA.System;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class ReadinessToBattleObserverInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ReadinessToBattleObserver>().AsSingle();
        }
    }
}