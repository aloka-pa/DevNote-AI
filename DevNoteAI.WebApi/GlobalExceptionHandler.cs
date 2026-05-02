using Microsoft.AspNetCore.Diagnostics;

namespace DevNoteAI.WebApi;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ArgumentException argumentException)
        {
            logger.LogWarning(exception, "Request validation failed.");
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new { message = argumentException.Message }, cancellationToken);
            return true;
        }

        logger.LogError(exception, "Unhandled exception.");
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." }, cancellationToken);
        return true;
    }
}
