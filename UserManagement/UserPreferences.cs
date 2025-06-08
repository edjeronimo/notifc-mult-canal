using System.Collections.Generic;
using System.Linq;

namespace NotificationSystem.UserManagement
{
    public class UserPreferences
    {
        public string UserId { get; set; } = string.Empty; 
        
        public Dictionary<NotificationType, List<NotificationChannel>> PreferredChannels { get; set; }
        public Dictionary<NotificationPriority, List<NotificationChannel>> PriorityChannels { get; set; }

        public UserPreferences()
        {
            PreferredChannels = new Dictionary<NotificationType, List<NotificationChannel>>();
            PriorityChannels = new Dictionary<NotificationPriority, List<NotificationChannel>>();
        }

        public List<NotificationChannel> GetChannelsForType(NotificationType type)
        {
            return PreferredChannels.GetValueOrDefault(type, new List<NotificationChannel>());
        }

        public List<NotificationChannel> GetChannelsForPriority(NotificationPriority priority)
        {
            return PriorityChannels.GetValueOrDefault(priority, new List<NotificationChannel>());
        }
    }
}