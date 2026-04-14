using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Electricity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ElectricityMeterConfiguration : IEntityTypeConfiguration<ElectricityMeter>
{
    public void Configure(EntityTypeBuilder<ElectricityMeter> builder)
    {
        builder.HasOne(em => em.Owner)
            .WithMany(m => m.ElectricityMeters)
            .HasForeignKey(em => em.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(em => em.Plot)
            .WithOne(p => p.ElectricityMeter)
            .HasForeignKey<ElectricityMeter>(em => em.PlotId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(em => em.Readings)
            .WithOne(r => r.Meter)
            .HasForeignKey(r => r.MeterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
