using GSManager.Core.Abstractions.Repository;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GSManager.Infrastructure.SQL;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddSqlInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("gsmanager-db");

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        return builder;
    }
}
