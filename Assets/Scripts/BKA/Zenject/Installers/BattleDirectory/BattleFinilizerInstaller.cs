using BKA.BattleDirectory.BattleSystems;
using Zenject;

namespace BKA.Zenject.Installers
{
    public class BattleFinilizerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BattleFinilizer>().AsSingle();
        }
    }
}