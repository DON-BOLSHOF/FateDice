using Zenject;

namespace BKA.Zenject.Installers
{
    public class DefinitionPoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DefinitionPool>().AsSingle();
        }
    }
}