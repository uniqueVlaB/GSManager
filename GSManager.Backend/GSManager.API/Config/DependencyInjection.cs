using FluentValidation;
using GSManager.API.ExceptionHandlers;
using GSManager.API.JsonConverters;
using GSManager.API.Telemetry;
using GSManager.Core.Options;
using GSManager.Infrastructure.SQL.Options;

namespace GSManager.API.Config;

/// <summary>
/// Configures dependency injection for the API layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsConfiguration(configuration);
        services.AddControllersConfiguration();
        services.AddCorsConfiguration();
        services.AddValidatorsConfiguration();
        services.AddExceptionHandlersConfiguration();
        services.AddTelemetryConfiguration();

        return services;
    }

    private static void AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("SqlServerDatabase"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
    }

    private static void AddControllersConfiguration(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new NullableGuidConverter()));
        services.AddOpenApi();
    }

    private static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options => options.AddDefaultPolicy(policy => policy
            .WithOrigins("http://localhost:4300")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));
    }

    private static void AddValidatorsConfiguration(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("GSManager.Core,", StringComparison.OrdinalIgnoreCase) == true)
            .ToArray();
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
    }

    private static void AddExceptionHandlersConfiguration(this IServiceCollection services)
    {
        services.AddExceptionHandler<GSManagerExceptionHandler>();
        services.AddExceptionHandler<DatabaseExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void AddTelemetryConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ApiMeters>();
    }
}
