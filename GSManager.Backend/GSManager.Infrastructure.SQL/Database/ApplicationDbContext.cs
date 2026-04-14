using GSManager.Core.Auth;
using GSManager.Core.Models.Entities.Auth;
using GSManager.Core.Models.Entities.Electricity;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Infrastructure.SQL.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Plot> Plots { get; set; }
    public DbSet<Priviledge> Priviledges { get; set; }
    public DbSet<Role> SocietyRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ElectricityMeter> ElectricityMeters { get; set; }
    public DbSet<ElectricityReading> ElectricityReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        EntityConfigurator.ConfigureEntities(builder);
        DatabaseSeeder.SeedDatabase(builder);
    }
}
