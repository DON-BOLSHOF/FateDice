using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    [CreateAssetMenu(menuName = "Installers/Battle/XPAwardingInstaller", fileName = "XPAwardingInstaller")]
    public class XPAwardingInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<XPAwarding>().AsSingle();
        }
    }
}