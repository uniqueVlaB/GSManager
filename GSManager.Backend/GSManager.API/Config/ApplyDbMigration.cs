using GSManager.Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;

namespace GSManager.API.Config;

public static class ApplyDbMigration
{
    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
