using Microsoft.AspNetCore.Authorization;

namespace GSManager.Core.Auth;

public class PermissionAuthRequirement(params string[] allowedPermissions) : IAuthorizationRequirement
{
    public string[] AllowedPermissions { get; } = allowedPermissions;
}

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthRequirement requirement)
    {
        if (context.User.FindFirst(c => c.Type == CustomClaimTypes.Permission && c.Value == Permissions.FullAccess) is not null)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        foreach (var permission in requirement.AllowedPermissions)
        {
            var found = context.User.FindFirst(c => c.Type == CustomClaimTypes.Permission && c.Value == permission) is not null;
            if (found)
            {
                context.Succeed(requirement);
                break;
            }
        }

        return Task.CompletedTask;
    }
}

public static class PermissionsExtensions
{
    public static void RequirePermission(this AuthorizationPolicyBuilder builder, params string[] allowedPermissions)
    {
        builder.AddRequirements(new PermissionAuthRequirement(allowedPermissions));
    }
}

