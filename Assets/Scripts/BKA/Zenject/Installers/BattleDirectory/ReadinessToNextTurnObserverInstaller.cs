using BKA.BattleDirectory.ReadinessObserver;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class ReadinessToNextTurnObserverInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ReadinessToNextTurnObservable>().AsSingle();
        }
    }
}