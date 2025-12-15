using System.Diagnostics.CodeAnalysis;
using GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Infrastructure.SQL.Database.Configuration;

[ExcludeFromCodeCoverage]
public static class EntityConfigurator
{
    public static void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlotConfiguration());
    }
}
