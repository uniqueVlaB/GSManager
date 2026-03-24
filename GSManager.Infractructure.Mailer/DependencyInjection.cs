using GSManager.Core.Abstractions.Mailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GSManager.Infrastructure.Mailer;

public static class DependencyInjection
{
    public static IServiceCollection AddMailerInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailerSettings>(configuration.GetSection("MailerSettings"));
        services.AddSingleton<MailQueue>();
        services.AddSingleton<IMailer, Mailer>();
        services.AddHostedService<MailerBackgroundService>();
        return services;
    }
}
