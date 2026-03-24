using System.Text;
using GSManager.Core.Auth;
using GSManager.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace GSManager.API.Config;

/// <summary>
/// Configures authentication and authorization for the application.
/// </summary>
public static class AuthConfiguration
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.AddJwtAuthentication();
        builder.AddPermissionAuthorization();
        return builder;
    }

    private static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()
            ?? throw new InvalidOperationException("JWT configuration is missing or invalid.");

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        });
    }

    private static void AddPermissionAuthorization(this WebApplicationBuilder builder)
    {
        var authBuilder = builder.Services.AddAuthorizationBuilder();

        foreach (var permission in Permissions.GetAllPermissions())
        {
            authBuilder.AddPolicy(permission, policy =>
                policy.RequireAuthenticatedUser().RequirePermission(permission));
        }

        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    }
}
