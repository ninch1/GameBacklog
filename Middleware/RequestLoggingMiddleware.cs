namespace GameBacklog.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"Before: {context.Request.Method} {context.Request.Path}");

        await next(context);

        Console.WriteLine($"After: {context.Response.StatusCode}");
    }
}