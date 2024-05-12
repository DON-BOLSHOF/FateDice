using BKA.UI.WorldMap;
using UnityEngine;

namespace Zenject.WorldMap
{
    public class NotificationPanelInstaller : MonoInstaller
    {
        [SerializeField] private NotificationPanel _notificationPanel;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NotificationPanel>().FromInstance(_notificationPanel).AsSingle();
        }
    }
}