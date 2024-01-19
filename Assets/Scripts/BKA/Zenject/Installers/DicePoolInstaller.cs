using BKA.Dices;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class DicePoolInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DicePool>().AsSingle();
        }
    }
}