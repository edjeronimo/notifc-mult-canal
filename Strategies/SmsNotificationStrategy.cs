using System;

namespace NotificationSystem.Strategies
{
    public class SmsNotificationStrategy : INotificationStrategy
    {
        public void Send(NotificationMessage message)
        {
            Console.WriteLine($"Enviando SMS para {message.Recipient}: {message.Content}");
            if (new Random().Next(0, 10) < 2)
            {
                throw new Exception("Falha temporária no envio de SMS (simulada).");
            }
        }
    }
}