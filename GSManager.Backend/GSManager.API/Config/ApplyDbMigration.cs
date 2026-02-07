using GSManager.Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;

namespace GSManager.API.Config;

public static class ApplyDbMigration
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
    }
}
