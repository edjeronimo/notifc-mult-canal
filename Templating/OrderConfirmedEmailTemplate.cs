using System.Collections.Generic;

namespace NotificationSystem.Templating
{
    public class OrderConfirmedEmailTemplate : INotificationTemplate
    {
        public string GenerateContent(Dictionary<string, string> data)
        {
            return $"Olá {data["nome_usuario"]}, seu pedido {data["id_pedido"]} foi {data["status_pedido"]}.";
        }
    }
}