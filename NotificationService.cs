using System;
using System.Collections.Generic;
using System.Linq;
using NotificationSystem.FaultHandling;
using NotificationSystem.Logging;
using NotificationSystem.Strategies;
using NotificationSystem.Templating;
using NotificationSystem.UserManagement;

namespace NotificationSystem
{
    public class NotificationService
    {
        private readonly Dictionary<NotificationChannel, INotificationStrategy> _strategies;
        private readonly INotificationLogger _logger;
        private readonly IRetryPolicy _retryPolicy;
        private readonly UserPreferencesService _userPreferencesService;

        public NotificationService(
            INotificationLogger logger,
            IRetryPolicy retryPolicy,
            UserPreferencesService userPreferencesService)
        {
            _strategies = new Dictionary<NotificationChannel, INotificationStrategy>
            {
                { NotificationChannel.Email, new EmailNotificationStrategy() },
                { NotificationChannel.SMS, new SmsNotificationStrategy() }
            };
            _logger = logger;
            _retryPolicy = retryPolicy;
            _userPreferencesService = userPreferencesService;
        }

        public void Notify(string userId, NotificationType type, NotificationPriority priority, Dictionary<string, string> data)
        {
            var userPreferences = _userPreferencesService.GetUserPreferences(userId);
            var possibleChannels = GetChannelsBasedOnPreferencesAndPriority(userPreferences, type, priority);

            if (!possibleChannels.Any())
            {
                _logger.LogNotification(new NotificationMessage { Recipient = userId, Type = type, Priority = priority, Data = data }, "Nenhum canal configurado para o usuário/preferência.");
                Console.WriteLine($"Nenhuma notificação enviada para {userId} para o tipo {type} e prioridade {priority}.");
                return;
            }

            foreach (var channel in possibleChannels)
            {
                try
                {
                    if (!_strategies.TryGetValue(channel, out INotificationStrategy? strategy))
                    {
                        _logger.LogNotification(new NotificationMessage { Recipient = userId, Type = type, Channel = channel, Priority = priority, Data = data }, $"Estratégia para canal {channel} não implementada/registrada.");
                        Console.WriteLine($"Erro: Estratégia para o canal {channel} não encontrada. Notificação não enviada.");
                        continue;
                    }

                    var template = NotificationTemplateFactory.CreateTemplate(type, channel);
                    var content = template.GenerateContent(data);
                    var message = new NotificationMessage
                    {
                        Recipient = userId,
                        Content = content,
                        Type = type,
                        Channel = channel,
                        Priority = priority,
                        Data = data
                    };

                    _retryPolicy.ExecuteWithRetries(() =>
                    {
                        strategy.Send(message);
                        _logger.LogNotification(message, "Sucesso");
                    }, (ex, attempt) =>
                    {
                        _logger.LogNotification(message, $"Falha (Tentativa {attempt}): {ex.Message}");
                        Console.WriteLine($"Tentativa {attempt} de envio de {type} via {channel} para {userId} falhou: {ex.Message}");
                    });

                }
                catch (Exception ex)
                {
                    _logger.LogNotification(new NotificationMessage { Recipient = userId, Type = type, Channel = channel, Priority = priority, Data = data }, $"Falha fatal ao processar: {ex.Message}");
                    Console.WriteLine($"Erro fatal ao processar notificação para {userId} via {channel}: {ex.Message}");
                }
            }
        }

        private List<NotificationChannel> GetChannelsBasedOnPreferencesAndPriority(UserPreferences preferences, NotificationType type, NotificationPriority priority)
        {
            var channelsForType = preferences.GetChannelsForType(type)!;
            var channelsForPriority = preferences.GetChannelsForPriority(priority)!;

            if (!channelsForType.Any() && !channelsForPriority.Any())
            {
                return new List<NotificationChannel>();
            }

            if (channelsForPriority.Any())
            {
                return channelsForType.Intersect(channelsForPriority).ToList();
            }

            return channelsForType;
        }
    }
}