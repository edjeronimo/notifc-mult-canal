using System.Collections.Generic;

namespace NotificationSystem.UserManagement
{
    public class UserPreferencesService
    {
        private readonly Dictionary<string, UserPreferences> _userPreferencesData;

        public UserPreferencesService()
        {
            _userPreferencesData = new Dictionary<string, UserPreferences>();
            var user1Prefs = new UserPreferences { UserId = "user123" };
            user1Prefs.PreferredChannels.Add(NotificationType.PedidoConfirmado, new List<NotificationChannel> { NotificationChannel.Email });
            user1Prefs.PreferredChannels.Add(NotificationType.SenhaAlterada, new List<NotificationChannel> { NotificationChannel.SMS, NotificationChannel.Email });
            user1Prefs.PreferredChannels.Add(NotificationType.PromocaoDisponivel, new List<NotificationChannel> { NotificationChannel.Email });
            user1Prefs.PriorityChannels.Add(NotificationPriority.Alta, new List<NotificationChannel> { NotificationChannel.SMS, NotificationChannel.Email });
            user1Prefs.PriorityChannels.Add(NotificationPriority.Media, new List<NotificationChannel> { NotificationChannel.Email });
            _userPreferencesData.Add("user123", user1Prefs);

            var user2Prefs = new UserPreferences { UserId = "user456" };
            user2Prefs.PreferredChannels.Add(NotificationType.PromocaoDisponivel, new List<NotificationChannel> { NotificationChannel.PushNotification });
            user2Prefs.PreferredChannels.Add(NotificationType.PedidoConfirmado, new List<NotificationChannel> { NotificationChannel.SMS });
            user2Prefs.PriorityChannels.Add(NotificationPriority.Alta, new List<NotificationChannel> { NotificationChannel.PushNotification });
            _userPreferencesData.Add("user456", user2Prefs);
        }

        public UserPreferences GetUserPreferences(string userId)
        {
            return _userPreferencesData.GetValueOrDefault(userId, new UserPreferences { UserId = userId });
        }
    }
}