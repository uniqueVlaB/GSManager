using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Electricity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Electricity;

[ExcludeFromCodeCoverage]
public class ElectricityReadingConfiguration : IEntityTypeConfiguration<ElectricityReading>
{
    public void Configure(EntityTypeBuilder<ElectricityReading> builder)
    {
        builder.Property(e => e.ReadingDay)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.ReadingNight)
            .HasColumnType("decimal(18,2)");
    }
}

