using System.Diagnostics.CodeAnalysis;
using GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Accounting;
using GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Auth;
using GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Electricity;
using GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Society;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Infrastructure.SQL.Database.Configuration;

[ExcludeFromCodeCoverage]
public static class EntityConfigurator
{
    public static void ConfigureEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlotConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new ElectricityMeterConfiguration());
        modelBuilder.ApplyConfiguration(new ElectricityReadingConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}
