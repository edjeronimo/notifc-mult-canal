using System;
using System.Collections.Generic;
using NotificationSystem.FaultHandling;
using NotificationSystem.Logging;
using NotificationSystem.UserManagement;

namespace NotificationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleNotificationLogger();
            var retryPolicy = new ExponentialBackoffRetryPolicy();
            var userPreferencesService = new UserPreferencesService();

            var notificationService = new NotificationService(logger, retryPolicy, userPreferencesService);

            Console.WriteLine("--- Sistema de Notificação Interativo ---");

            while (true)
            {
                Console.WriteLine("\nSelecione uma opção:");
                Console.WriteLine("1. Enviar Nova Notificação");
                Console.WriteLine("2. Executar Cenários de Teste Pré-definidos");
                Console.WriteLine("3. Sair");
                Console.Write("Opção: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SendNewNotification(notificationService);
                        break;
                    case "2":
                        RunPredefinedScenarios(notificationService);
                        break;
                    case "3":
                        Console.WriteLine("Saindo do sistema. Até mais!");
                        return;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
        }

        static void SendNewNotification(NotificationService notificationService)
        {
            Console.WriteLine("\n--- Enviar Nova Notificação ---");

            Console.Write("Digite o ID do usuário: ");
            var userId = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine("ID do usuário não pode ser vazio.");
                return;
            }

            NotificationType notificationType;
            while (true)
            {
                Console.WriteLine("Selecione o Tipo de Notificação:");
                foreach (NotificationType type in Enum.GetValues(typeof(NotificationType)))
                {
                    Console.WriteLine($"  {(int)type}. {type}");
                }
                Console.Write("Tipo (número): ");
                if (int.TryParse(Console.ReadLine(), out int typeInt) && Enum.IsDefined(typeof(NotificationType), typeInt))
                {
                    notificationType = (NotificationType)typeInt;
                    break;
                }
                Console.WriteLine("Tipo de notificação inválido. Tente novamente.");
            }

            NotificationPriority notificationPriority;
            while (true)
            {
                Console.WriteLine("Selecione a Prioridade:");
                foreach (NotificationPriority priority in Enum.GetValues(typeof(NotificationPriority)))
                {
                    Console.WriteLine($"  {(int)priority}. {priority}");
                }
                Console.Write("Prioridade (número): ");
                if (int.TryParse(Console.ReadLine(), out int priorityInt) && Enum.IsDefined(typeof(NotificationPriority), priorityInt))
                {
                    notificationPriority = (NotificationPriority)priorityInt;
                    break;
                }
                Console.WriteLine("Prioridade inválida. Tente novamente.");
            }

            var customData = new Dictionary<string, string>();
            Console.WriteLine("Adicione dados personalizados (digite 'fim' para terminar):");
            while (true)
            {
                Console.Write("Chave (ou 'fim'): ");
                var key = Console.ReadLine();
                if (key?.ToLower() == "fim") break;
                if (string.IsNullOrWhiteSpace(key))
                {
                    Console.WriteLine("Chave não pode ser vazia.");
                    continue;
                }

                Console.Write($"Valor para '{key}': ");
                var value = Console.ReadLine() ?? string.Empty;
                customData[key] = value;
            }

            Console.WriteLine($"\nEnviando notificação para {userId}...");
            notificationService.Notify(userId, notificationType, notificationPriority, customData);
            Console.WriteLine("Solicitação de notificação enviada. Verifique os logs acima para o status.");
        }

        static void RunPredefinedScenarios(NotificationService notificationService)
        {
            Console.WriteLine("\n--- Executando Cenários de Teste Pré-definidos ---");

            Console.WriteLine("\n--- Cenário 1: Pedido Confirmado para user123 (Email, Prioridade Média) ---");
            notificationService.Notify("user123", NotificationType.PedidoConfirmado, NotificationPriority.Media, new Dictionary<string, string>
            {
                { "nome_usuario", "João" },
                { "id_pedido", "ABC-123" },
                { "status_pedido", "confirmado" }
            });

            Console.WriteLine("\n--- Cenário 2: Senha Alterada para user123 (SMS, Email, Prioridade Alta) ---");
            notificationService.Notify("user123", NotificationType.SenhaAlterada, NotificationPriority.Alta, new Dictionary<string, string>
            {
                { "nome_usuario", "João" }
            });

            Console.WriteLine("\n--- Cenário 3: Promoção Disponível para user456 (PushNotification, Prioridade Baixa) ---");
            notificationService.Notify("user456", NotificationType.PromocaoDisponivel, NotificationPriority.Baixa, new Dictionary<string, string>
            {
                { "nome_usuario", "Maria" },
                { "desconto", "15%" }
            });

            Console.WriteLine("\n--- Cenário 4: Teste de Retentativa para user123 (Email, Prioridade Média - pode falhar e tentar novamente) ---");
            notificationService.Notify("user123", NotificationType.PedidoConfirmado, NotificationPriority.Media, new Dictionary<string, string>
            {
                { "nome_usuario", "Pedro" },
                { "id_pedido", "XYZ-789" },
                { "status_pedido", "enviado" }
            });

            Console.WriteLine("\n--- Cenário 5: Notificação para user789 (sem preferências configuradas) ---");
            notificationService.Notify("user789", NotificationType.PedidoConfirmado, NotificationPriority.Media, new Dictionary<string, string>
            {
                { "nome_usuario", "Carlos" },
                { "id_pedido", "DEF-456" },
                { "status_pedido", "em processamento" }
            });

            Console.WriteLine("\n--- Cenários de Teste Pré-definidos Concluídos ---");
        }
    }
}