using System;
using System.Threading;

namespace NotificationSystem.FaultHandling
{
    public class ExponentialBackoffRetryPolicy : IRetryPolicy
    {
        private readonly int _maxRetries;
        private readonly int _initialDelayMs;

        public ExponentialBackoffRetryPolicy(int maxRetries = 3, int initialDelayMs = 500)
        {
            _maxRetries = maxRetries;
            _initialDelayMs = initialDelayMs;
        }

        public void ExecuteWithRetries(Action action, Action<Exception, int> onRetry)
        {
            int attempt = 0;
            while (attempt < _maxRetries)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    attempt++;
                    onRetry(ex, attempt);

                    if (attempt < _maxRetries)
                    {
                        Thread.Sleep(_initialDelayMs * (int)Math.Pow(2, attempt - 1));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}