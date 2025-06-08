using System;

namespace NotificationSystem.FaultHandling
{
    public interface IRetryPolicy
    {
        void ExecuteWithRetries(Action action, Action<Exception, int> onRetry);
    }
}