using System.Text.Json;
using System.Text.Json.Serialization;
using api.Exceptions;
using api.Utilities;
using Microsoft.AspNetCore.Diagnostics;

namespace api.Services
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web)
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        // This method is used to handle exceptions
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var details = exception switch
            {
                BaseException baseException => new ProblemDetails
                {
                    Status = (int)baseException.Code,
                    Title = baseException.Title,
                    Detail = baseException.Message
                },
                _ => new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred"
                }
            };
            details.TraceId = httpContext.TraceIdentifier;
            details.Data = exception.Data;

            var json = JsonSerializer.Serialize(details, _jsonSerializerOptions);
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = details.Status;
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}