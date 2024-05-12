namespace BKA.WorldMapDirectory
{
    public interface INotificationHandler
    {
        void LoadNotificationObject(INotifingObject notifingObject);
        void RemoveNotificationObject(INotifingObject notifingObject);
    }
}