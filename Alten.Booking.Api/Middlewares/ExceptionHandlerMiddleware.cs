using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Alten.Booking.Api.Middlewares
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;

            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                var error = new { error_message = exception.Message };

                _logger.LogError(exception, $"{nameof(ExceptionHandlerMiddleware)}: {exception.Message}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = GetStatusCode(exception);
                await context.Response.WriteAsync(JsonSerializer.Serialize(error, _jsonSerializerOptions));
            }
        }

        private int GetStatusCode(Exception ex)
        {
            if (ex is ArgumentException || ex is ArgumentOutOfRangeException)
                return (int)HttpStatusCode.BadRequest;
            if (ex is ApplicationException || ex is InvalidOperationException)
                return (int)HttpStatusCode.UnprocessableEntity;

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
