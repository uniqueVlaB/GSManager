using GSManager.Core.Abstractions.Services.Auth;
using GSManager.Core.Abstractions.Services.Electricity;
using GSManager.Core.Abstractions.Services.Society;
using GSManager.Core.Services;
using GSManager.Core.Services.Auth;
using GSManager.Core.Services.Electricity;
using GSManager.Core.Services.Society;
using Microsoft.Extensions.DependencyInjection;

namespace GSManager.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Application services
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IPlotService, PlotService>();
        services.AddScoped<IElectricityMeterService, ElectricityMeterService>();
        services.AddScoped<IPriviledgeService, PriviledgeService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
