namespace NotificationSystem.Strategies
{
    public interface INotificationStrategy
    {
        void Send(NotificationMessage message);
    }
}