namespace BKA.UI.WorldMap
{
    public class Notification
    {
        public string Description { get; }
        public string RequestDescription { get; }

        public Notification(string description, string requestDescription)
        {
            Description = description;
            RequestDescription = requestDescription;
        }
    }
}