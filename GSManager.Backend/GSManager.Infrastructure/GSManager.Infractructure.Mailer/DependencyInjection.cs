using GSManager.Core.Abstractions.Mailer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSManager.Infrastructure.Mailer;

public static class DependencyInjection
{
    public static IServiceCollection AddMailerInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMailer, Mailer>();

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var connectionString = configuration.GetConnectionString("rabbit-mq");

                if (!string.IsNullOrEmpty(connectionString))
                {
                    cfg.Host(connectionString);
                }

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
