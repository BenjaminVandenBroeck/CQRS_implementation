using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CQRS_Example.Common.ReadStore
{
    public static class PollyExtensions
    {
        public static async Task ExecuteChangeFeedPolicy(Func<Task>action, ILogger logger)
        {
            var context = new Context(Guid.NewGuid().ToString(), new Dictionary<string, object>
            {
                {"logger", logger }
            });

            var policy = GetchangeFeedPolicy();
            await policy.ExecuteAsync(async _ =>
            {
                await action();
            }, context);
        }

        private static AsyncRetryPolicy GetchangeFeedPolicy()
        {
            var policy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    if (!context.TryGetLogger(out var logger)) return;

                    logger.LogWarning("Retry {retryCount} encountered an error: {ex}. Waiting {timeSpan} before next retry.", retryCount, exception.Message, timeSpan);
                });
            return policy;
        }

        private static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if(context.TryGetValue("logger", out var loggerObject) && loggerObject is ILogger theLogger)
            {
                logger = theLogger;
                return true;
            }
            logger = null;
            return false;
        }
    }
}
