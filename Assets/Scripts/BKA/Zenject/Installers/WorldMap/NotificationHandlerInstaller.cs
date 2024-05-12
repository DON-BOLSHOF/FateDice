using BKA.WorldMapDirectory.Quest;
using UnityEngine;

namespace Zenject.WorldMap
{
    [CreateAssetMenu(menuName = "Installers/WorldMap/NotificationHandlerInstaller", fileName = "NotificationHandlerInstaller")]
    public class NotificationHandlerInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NotificationHandler>().AsSingle();
        }
    }
}