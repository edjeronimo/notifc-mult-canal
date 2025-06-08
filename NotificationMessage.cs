// NotificationMessage.cs
using System.Collections.Generic;

namespace NotificationSystem
{
    public class NotificationMessage
    {
        public string Recipient { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;  
        public NotificationType Type { get; set; }
        public NotificationChannel Channel { get; set; }
        public NotificationPriority Priority { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}