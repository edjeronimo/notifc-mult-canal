using System;
using System.Collections.Generic;

namespace NotificationSystem.Templating
{
    public static class NotificationTemplateFactory
    {
        public static INotificationTemplate CreateTemplate(NotificationType type, NotificationChannel channel)
        {
            switch (type)
            {
                case NotificationType.PedidoConfirmado:
                    if (channel == NotificationChannel.Email) return new OrderConfirmedEmailTemplate();
                    break;
                case NotificationType.SenhaAlterada:
                    if (channel == NotificationChannel.SMS) return new PasswordChangedSmsTemplate();
                    break;
                case NotificationType.PromocaoDisponivel:
                    if (channel == NotificationChannel.PushNotification) return new GenericPushNotificationTemplate();
                    break;
            }
            throw new ArgumentException($"Template não encontrado para o tipo {type} e canal {channel}.");
        }
    }

    public class GenericPushNotificationTemplate : INotificationTemplate
    {
        public string GenerateContent(Dictionary<string, string> data)
        {
            return $"Uma nova notificação para você! Detalhes: {string.Join(", ", data.Values)}";
        }
    }
}