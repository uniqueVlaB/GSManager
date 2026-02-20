using System.Security.Claims;
using GSManager.Core.Auth;
using Microsoft.AspNetCore.Identity;

namespace GSManager.API.Config;

internal static class SeedDefaultIdentity
{
    internal static async Task SeedDefaultIdentityAsync(this IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAsync(roleManager);
        await SeedAdminUserAsync(userManager);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        // Admin role with full access
        if (await roleManager.FindByNameAsync(Roles.Admin) is null)
        {
            var adminRole = new IdentityRole<Guid>(Roles.Admin);
            await roleManager.CreateAsync(adminRole);
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Permissions.FullAccess));
        }

        // Member role with limited access
        if (await roleManager.FindByNameAsync(Roles.Member) is null)
        {
            var memberRole = new IdentityRole<Guid>(Roles.Member);
            await roleManager.CreateAsync(memberRole);
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.ViewMembers));
            await roleManager.AddClaimAsync(memberRole, new Claim(CustomClaimTypes.Permission, Permissions.ViewPlots));
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@example.com";

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
        {
            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            MemberId = Guid.Empty,
        };

        await userManager.CreateAsync(adminUser, "Admin@123");
        await userManager.AddToRoleAsync(adminUser, Roles.Admin);
    }
}
