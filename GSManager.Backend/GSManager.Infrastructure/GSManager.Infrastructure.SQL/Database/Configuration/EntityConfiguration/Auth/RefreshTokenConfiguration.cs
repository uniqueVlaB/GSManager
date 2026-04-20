using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GSManager.Infrastructure.SQL.Database.Configuration.EntityConfiguration.Auth;

[ExcludeFromCodeCoverage]
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Token).HasMaxLength(256);
        builder.HasIndex(p => p.Token).IsUnique();
    }
}

