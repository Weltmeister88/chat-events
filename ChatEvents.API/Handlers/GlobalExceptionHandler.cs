using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ChatEvents.API.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment hostEnvironment)
    : IExceptionHandler
{
    private const string UnhandledExceptionMessage = "An unhandled exception has occurred while executing the request.";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.ToString());

        ProblemDetails problemDetails = CreateProblemDetails(httpContext, exception);
        string json = ToJson(problemDetails);

        const string contentType = "application/json";
        httpContext.Response.ContentType = contentType;
        await httpContext.Response.WriteAsync(json, cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        return new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = UnhandledExceptionMessage,
            Detail = hostEnvironment.IsProduction() ? null : exception.ToString()
        };
    }
    
    private string ToJson(ProblemDetails problemDetails)
    {
        try
        {
            return JsonSerializer.Serialize(problemDetails);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
        return UnhandledExceptionMessage;
    }
}