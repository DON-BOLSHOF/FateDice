using BKA.BootsTraps;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class BootsTrapStateObserverInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootsTrapStateObserver>().AsSingle();
        }
    }
}