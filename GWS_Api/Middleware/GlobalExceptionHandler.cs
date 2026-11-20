using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GWS_Api.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Abgebrochene Blazor-Requests ruhig als Info loggen (kein Fehler-Spam)
        if (exception is OperationCanceledException)
        {
            logger.LogInformation("Der Client hat die Anfrage vorzeitig abgebrochen.");
            httpContext.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
            return true;
        }

        // 2. Alle anderen unerwarteten Fehler mit StackTrace loggen
        logger.LogError(exception, "Ein unerwarteter Fehler ist aufgetreten: {Message}", exception.Message);

        // 3. Standardisierte Fehlerantwort (RFC 7807) für den Client bauen
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Serverfehler",
            Detail = "Ein interner Fehler ist aufgetreten. Bitte versuchen Sie es später erneut."
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Signalisiert ASP.NET Core: Fehler wurde vollständig behandelt
    }
}
