using BKA.System;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class SynchronizerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BattleSynchronizer>().AsSingle();
        }
    }
}