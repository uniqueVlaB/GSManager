using FluentValidation;
using GSManager.API.ExceptionHandlers;
using GSManager.API.JsonConverters;
using GSManager.API.Telemetry;

namespace GSManager.API.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new NullableGuidConverter()));
        services.AddOpenApi();

        services.AddCors(options => options.AddDefaultPolicy(policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith("GSManager.Core,", StringComparison.OrdinalIgnoreCase) == true)
            .ToArray();
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

        services.AddExceptionHandler<GSManagerExceptionHandler>();
        services.AddExceptionHandler<DatabaseExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // Custom metrics
        services.AddSingleton<ApiMeters>();

        return services;
    }
}
