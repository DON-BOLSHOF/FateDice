using Zenject;

namespace BKA.Zenject.Installers
{
    public class TurnSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TurnSystem>().AsSingle();
        }
    }
}