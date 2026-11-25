using Microsoft.AspNetCore.Diagnostics;

namespace GameBacklog.Middleware;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is BadHttpRequestException badRequest)
        {
            logger.LogWarning(exception, "Invalid request.");

            await Results.Problem(
                detail: "Check that the request body contains the expected data types."
,
                statusCode: badRequest.StatusCode,
                title: "Invalid request").ExecuteAsync(context);

            return true;
        }

        logger.LogError(exception, "An unhandled error occurred.");

        await Results.Problem(
            statusCode: 500,
            title: "An unexpected error occurred."
        ).ExecuteAsync(context);

        return true;
    }
}