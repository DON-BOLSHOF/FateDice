using BKA.BattleDirectory.BattleSystems;
using UnityEngine;
using Zenject;

namespace BKA.Zenject.Installers
{
    [CreateAssetMenu(menuName = "Installers/Battle/ArtefactAwardingInstaller", fileName = "ArtefactAwardingInstaller")]
    public class ArtefactAwardingInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ArtefactAwarding>().AsSingle();
        }
    }
}