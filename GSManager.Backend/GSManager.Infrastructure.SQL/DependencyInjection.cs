using GSManager.Core.Abstractions.Repository;
using GSManager.Infrastructure.SQL.Database;
using GSManager.Infrastructure.SQL.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GSManager.Infrastructure.SQL;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlInfrastructureServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            switch (dbOptions.Provider)
            {
                case "SqlServer":
                    options.UseSqlServer(dbOptions.ConnectionString);
                    break;

                // Add other providers here as needed
                default:
                    throw new InvalidOperationException($"Unsupported provider: {dbOptions.Provider}");
            }
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
