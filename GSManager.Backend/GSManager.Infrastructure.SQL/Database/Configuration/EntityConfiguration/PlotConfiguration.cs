using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Society;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class PlotConfiguration : IEntityTypeConfiguration<Plot>
{
    public void Configure(EntityTypeBuilder<Plot> builder)
    {
        builder.HasOne(p => p.Owner)
           .WithMany(m => m.Plots)
           .HasForeignKey(p => p.OwnerId)
           .OnDelete(DeleteBehavior.SetNull);

        builder.Property(p => p.Number).IsRequired();
        builder.HasIndex(p => p.Number).IsUnique();
    }
}
