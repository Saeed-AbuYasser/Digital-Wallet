using DigitalWallet.Domain.Exceptions;
using System.Net;
using System.Text.Json;
namespace DigitalWallet.API.Middlewares
{

    public record ErrorResponse(int StatusCode, string Message, string? Details = null);
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Let the request proceed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // This is the "Mapping" logic
            var (statusCode, message) = exception switch
            {
                // 400 Bad Request for business rule violations
                DataCorrectnessViolationException or DataValueBiggerThanExpectedException  or RequiredFieldMissingException
                => (StatusCodes.Status400BadRequest, exception.Message),

                // 404 Not Found
                NotFoundException or ForeignKeyViolationException => (StatusCodes.Status404NotFound, exception.Message),


                // 401 Unauthorized
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "You are not authorized."),

                // 409 Conflict for concurrency issues
                ArgumentException or DuplicateValueException => (StatusCodes.Status409Conflict, exception.Message),

                // Default 500 for everything else (the "Scary" errors)
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred on the server.")
            };

            context.Response.StatusCode = statusCode;

            // Use the model we created earlier
            var response = new ErrorResponse(
                StatusCode: statusCode,
                Message: message,
                Details: context.Request.Host.Host.Contains("localhost") ? exception.StackTrace : null
            );

            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
        }
    }

}
