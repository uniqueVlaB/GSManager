using GSManager.API.Middleware;

namespace GSManager.API.Config;

public static class MiddlewareConfiguration
{
    public static void ConfigureCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
    }
}
