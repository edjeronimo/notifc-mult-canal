namespace NotificationSystem.Logging
{
    public interface INotificationLogger
    {
        void LogNotification(NotificationMessage message, string status);
    }
}