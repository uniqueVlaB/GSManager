using GSManager.Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GSManager.Infrastructure.SQL.Migrations.SqlServer;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=GSManager;Trusted_Connection=True;TrustServerCertificate=True",
                x => x.MigrationsAssembly("GSManager.Infrastructure.SQL.Migrations.SqlServer"))
            .Options;

        return new ApplicationDbContext(options);
    }
}
