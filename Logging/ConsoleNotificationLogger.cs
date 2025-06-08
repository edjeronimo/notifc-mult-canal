using System;

namespace NotificationSystem.Logging
{
    public class ConsoleNotificationLogger : INotificationLogger
    {
        public void LogNotification(NotificationMessage message, string status)
        {
            int contentHash = message.Content?.GetHashCode() ?? 0;
            Console.WriteLine($"LOG: User: {message.Recipient}, Type: {message.Type}, Channel: {message.Channel}, Priority: {message.Priority}, Status: {status}, Content Hash: {contentHash}");
        }
    }
}