using System.Collections.Generic;
using BKA.UI.WorldMap;
using UniRx;

namespace BKA.WorldMapDirectory.Quest
{
    public class NotificationHandler : INotificationHandler
    {
        private class DisposableNotificationObject
        {
            public readonly CompositeDisposable NotificationDisposable = new();
            public readonly INotifingObject NotifingObject;

            public DisposableNotificationObject(INotifingObject notifingObject)
            {
                NotifingObject = notifingObject;
            }
        }

        private List<DisposableNotificationObject> _notifingObjects = new();

        private INotificationPanel _notificationPanel;

        public NotificationHandler(INotificationPanel notificationPanel)
        {
            _notificationPanel = notificationPanel;
        }

        public void LoadNotificationObject(INotifingObject notifingObject)
        {
            var disposeObject = new DisposableNotificationObject(notifingObject);

            _notifingObjects.Add(disposeObject);

            disposeObject.NotifingObject.OnSentNotification
                .Subscribe(notification => _notificationPanel.SentNotification(notification))
                .AddTo(disposeObject.NotificationDisposable);
        }

        public void RemoveNotificationObject(INotifingObject notifingObject)
        {
            var temp = _notifingObjects.Find(o => o.NotifingObject.Equals(notifingObject));
            temp.NotificationDisposable.Dispose();
            _notifingObjects.Remove(temp);
        }
    }
}