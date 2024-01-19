using BKA.Units;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class UnitBattleBehaviourUploaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UnitBattleBehaviourUploader>().AsSingle();
        }
    }
}