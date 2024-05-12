using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/DialogElementsMediatorInstaller", fileName = "DialogElementsMediatorInstaller")]
    public class DialogElementsMediatorInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DialogElementsMediator>().AsSingle();
        }
    }
}