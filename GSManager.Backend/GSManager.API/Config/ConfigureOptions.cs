using GSManager.Core.Options;
using GSManager.Infrastructure.SQL.Options;

namespace GSManager.API.Config;

public static class ConfigureOptions
{
    public static void ConfigureOptionsPatterns(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("SqlServerDatabase"));
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
    }
}
