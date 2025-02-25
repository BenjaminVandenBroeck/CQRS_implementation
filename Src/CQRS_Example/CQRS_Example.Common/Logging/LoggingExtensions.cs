using CQRS_Example.Common.ExceptionHandling;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Common.Logging
{
    public static class LoggingExtensions
    {
        public static BaseException LogAndThrowDomainError(this ILogger logger, string message, Exception ex, string errorIdentifier, ExceptionType type, params object[] parameters)
        {
            var logParams= new object[parameters.Length+1];
            parameters.CopyTo(logParams, 0);
            logParams[^1] = ex.ToString();
            logger.LogError(message, logParams);

            var @params= new object[parameters.Length+1];
            parameters.CopyTo(@params, 0);
            @params[^1] = ex.Message;
            return new BaseException(message, errorIdentifier, type, @params);
        }

        public static BaseException LogAndThrowDomainError(this ILogger logger, string message, Exception ex, string errorIdentifier, params object[] parameters)
        {
            return LogAndThrowDomainError(logger, message, ex, errorIdentifier, ExceptionType.General, parameters);
        }

        public static BaseException LogAndThrowDomainError(this ILogger logger, string message, string errorIdentifier, ExceptionType type, params object[] parameters)
        {
            logger.LogError(message, parameters);
            return new BaseException(message,errorIdentifier, type, parameters);    
        }

        public static BaseException LogAndThrowDomainError(this ILogger logger, string message, string errorIdentifier, params object[] parameters)
        {
            return LogAndThrowDomainError(logger, message, errorIdentifier, ExceptionType.General, parameters);
        }

    }
}
