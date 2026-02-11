using System.Security.Claims;
using GSManager.Core.Auth;
using Microsoft.AspNetCore.Identity;

namespace GSManager.API.Config;

public static class SeedDefaultIdentity
{
    public static async Task SeedDefaultIdentityAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
        if (adminRole == null)
        {
            await roleManager.CreateAsync(adminRole = new IdentityRole<Guid>(Roles.Admin));

            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.FullAccess));
        }

        var memberRole = await roleManager.FindByNameAsync(Roles.Member);
        if (memberRole == null)
        {
            await roleManager.CreateAsync(memberRole = new IdentityRole<Guid>(Roles.Member));

            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.ViewMembers));
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.ViewPlots));
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                MemberId = Guid.Empty,
            };

            await userManager.CreateAsync(adminUser, "Admin@123");

            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
    }
}
