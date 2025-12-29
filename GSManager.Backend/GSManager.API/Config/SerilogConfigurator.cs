using GSManager.Core.Exceptions;
using Serilog;

namespace GSManager.API.Config;

public static class SerilogConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
#pragma warning disable CA1305 // Specify IFormatProvider
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Information)
                    .WriteTo.File(
                        path: "Logs/Info/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == Serilog.Events.LogEventLevel.Warning)
                    .WriteTo.File(
                        path: "Logs/Exceptions/Warnings/warnings-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e =>
                        e.Level >= Serilog.Events.LogEventLevel.Error && e.Exception is not GSManagerException)
                    .WriteTo.File(
                        path: "Logs/Exceptions/Errors/errors-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"));
        });
#pragma warning restore CA1305 // Specify IFormatProvider
    }
}
