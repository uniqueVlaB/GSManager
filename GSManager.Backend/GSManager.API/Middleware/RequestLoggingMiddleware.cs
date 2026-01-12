using System.Text;

namespace GSManager.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        ValidateParameters(context);
        await HandleRequestAsync(context);
    }

    private static void ValidateParameters(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }

    private async Task HandleRequestAsync(HttpContext context)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        var requestContent = string.Empty;

        // Only log body for methods that typically have one
        if (context.Request.ContentLength > 0 &&
            context.Request.Body.CanRead &&
            (context.Request.Method == HttpMethods.Post ||
             context.Request.Method == HttpMethods.Put ||
             context.Request.Method == HttpMethods.Patch))
        {
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestContent = await reader.ReadToEndAsync();
            }

            context.Request.Body.Position = 0;
        }

        // Capture response
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var statusCode = context.Response.StatusCode;
            var requestMethod = context.Request.Method;
            var elapsedMs = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation(
                "Request from IP: {IpAddress}, URL: {Url}, Method: {Method}, StatusCode: {StatusCode}, Elapsed: {ElapsedMs}ms\nRequestContent: {RequestContent}\nResponseContent: {ResponseContent}\n",
                ipAddress,
                url,
                requestMethod,
                statusCode,
                elapsedMs,
                requestContent,
                responseContent);

            // Copy response back to original stream
            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }
}
