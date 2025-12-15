using System.Diagnostics.CodeAnalysis;
using GSManager.Core.Models.Entities.Society;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Infrastructure.SQL.Database.Configuration;

[ExcludeFromCodeCoverage]
public static class DatabaseSeeder
{
    // Deterministic seed GUIDs
    private static readonly Guid PrivBaseId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid PrivPensionId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid RoleGardenerId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    private static readonly Guid MemberSeedId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    private static readonly Guid Plot1Id = Guid.Parse("55555555-5555-5555-5555-555555555555");
    private static readonly Guid Plot2Id = Guid.Parse("66666666-6666-6666-6666-666666666666");
    private static readonly Guid Plot3Id = Guid.Parse("77777777-7777-7777-7777-777777777777");

    public static void SeedDatabase(ModelBuilder modelBuilder)
    {
        SeedPriviledges(modelBuilder);
        SeedRoles(modelBuilder);
        SeedMembers(modelBuilder);
        SeedPlots(modelBuilder);
    }

    public static void SeedPriviledges(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Priviledge>().HasData(
            new Priviledge { Id = PrivBaseId, Name = "Звичайні тарифи", Description = "База" },
            new Priviledge { Id = PrivPensionId, Name = "Пільга для пенсіонерів", Description = "Саме за себе говорить" });
    }

    public static void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = RoleGardenerId, Name = "Садівник", Description = "Звичайний садівник, нічого цікавого", PriviledgeId = PrivBaseId });
    }

    public static void SeedMembers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>().HasData(
            new Member
            {
                Id = MemberSeedId,
                FirstName = "Зрадослав",
                MiddleName = "Потужнович",
                LastName = "Жабагадюк",
                PhoneNumber = "+3803455347645",
                Email = "email@email.ua",
                RoleId = RoleGardenerId,
                PriviledgeId = PrivBaseId,
            });
    }

    public static void SeedPlots(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plot>().HasData(
            new Plot { Id = Plot1Id, Number = "1", OwnerId = MemberSeedId },
            new Plot { Id = Plot2Id, Number = "2", OwnerId = MemberSeedId },
            new Plot { Id = Plot3Id, Number = "3" });
    }
}
