using GSManager.Core.Abstractions.Services;
using GSManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GSManager.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Application services
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPlotService, PlotService>();
        services.AddScoped<IPriviledgeService, PriviledgeService>();

        return services;
    }
}
