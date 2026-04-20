using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Accounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Accounting;

[ExcludeFromCodeCoverage]
public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaymentType)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(p => p.Member)
            .WithMany(m => m.Payments)
            .HasForeignKey(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

