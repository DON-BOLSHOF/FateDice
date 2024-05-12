using System;
using BKA.UI.WorldMap;

namespace BKA.WorldMapDirectory
{
    public interface INotifingObject
    {
        IObservable<Notification> OnSentNotification { get; }
    }
}