using CQRS_Example.API.Model.Base;
using CQRS_Example.Common.ExceptionHandling;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CQRS_Example.API.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex) when (ex is BaseException baseException)
            {
                await HandleBaseExceptionAsync(context, baseException);
            }
            catch (Exception ex)
            {
                await HandleUnexpectedExceptionAsync(context, ex);
            }
        }

        private Task HandleBaseExceptionAsync(HttpContext context, BaseException ex)
        {
            var envelope = new ErrorEnvelope(ex.Message, ex.ErrorIdentifier);
            _logger.LogError(exception: ex, message: ex.Message);
            return ReturnException(context, envelope, (int)ex.Type);
        }

        private Task HandleUnexpectedExceptionAsync(HttpContext context, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine("An unexexpected error happened: ");
            sb.AppendLine(ex.Message);
            var envelope = new ErrorEnvelope(sb.ToString(), "unexpected");
            _logger.LogError(exception: ex, message: ex.Message);
            return ReturnException(context, envelope, (int)HttpStatusCode.InternalServerError);
        }

        private Task ReturnException(HttpContext context, ErrorEnvelope errorEnvelope, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result= JsonSerializer.Serialize(errorEnvelope);
            return context.Response.WriteAsync(result);
        }

    }
}
