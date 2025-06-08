using System.Collections.Generic;

namespace NotificationSystem.Templating
{
    public class PasswordChangedSmsTemplate : INotificationTemplate
    {
        public string GenerateContent(Dictionary<string, string> data)
        {
            return $"Sua senha foi alterada com sucesso. Se não foi você, contate-nos imediatamente.";
        }
    }
}