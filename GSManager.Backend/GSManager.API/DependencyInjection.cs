using GSManager.API.ExceptionHandlers;
using GSManager.API.JsonConverters;

namespace GSManager.API;

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

        services.AddExceptionHandler<GSManagerExceptionHandler>();
        services.AddExceptionHandler<DatabaseExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
