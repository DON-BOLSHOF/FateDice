using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    [CreateAssetMenu(menuName = "Installers/Battle/WinHandlerInstaller", fileName = "WinHandlerInstaller")]
    public class WinHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WinHandler>().AsSingle();
        }
    }
}