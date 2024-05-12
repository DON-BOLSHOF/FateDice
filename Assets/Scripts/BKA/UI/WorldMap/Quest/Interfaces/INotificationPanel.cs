namespace BKA.UI.WorldMap
{
    public interface INotificationPanel
    {
        void SentNotification(Notification notification);
        void SentNotification(Notification[] notifications);
    }
}