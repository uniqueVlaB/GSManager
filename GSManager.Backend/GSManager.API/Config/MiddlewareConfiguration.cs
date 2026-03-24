using GSManager.API.Middleware;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;

namespace GSManager.API.Config;

/// <summary>
/// Configures the middleware pipeline for the application.
/// </summary>
public static class MiddlewareConfiguration
{
    public static WebApplication UseCustomMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        return app;
    }

    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await scope.ServiceProvider.SeedDefaultIdentityAsync();
    }
}
