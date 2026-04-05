using DeviceManager.Core.DTOs;
using DeviceManager.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DeviceManager.API.Middleware;

/// <summary>
/// Global exception handler catching all unhandled exceptions.
/// Formats errors consistently as ErrorResponse JSON objects.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var (statusCode, message) = MapExceptionToStatusCodeAndMessage(exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            Details = httpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment()
                ? exception.StackTrace
                : null
        };

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        
        // Return true to indicate the request was handled here
        return true;
    }

    private static (int StatusCode, string Message) MapExceptionToStatusCodeAndMessage(Exception exception)
    {
        return exception switch
        {
            NotFoundException notFoundException => (StatusCodes.Status404NotFound, notFoundException.Message),
            ConflictException conflictException => (StatusCodes.Status409Conflict, conflictException.Message),
            UnauthorizedException unauthorizedException => (StatusCodes.Status401Unauthorized, unauthorizedException.Message),
            ForbiddenException forbiddenException => (StatusCodes.Status403Forbidden, forbiddenException.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };
    }
}
