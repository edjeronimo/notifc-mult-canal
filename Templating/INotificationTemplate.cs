using System.Collections.Generic;

namespace NotificationSystem.Templating
{
    public interface INotificationTemplate
    {
        string GenerateContent(Dictionary<string, string> data);
    }
}