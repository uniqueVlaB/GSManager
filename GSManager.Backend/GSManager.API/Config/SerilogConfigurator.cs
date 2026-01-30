using GSManager.Core.Exceptions;
using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace GSManager.API.Config;

public static class SerilogConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        var otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

#pragma warning disable CA1305 // Specify IFormatProvider
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
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

            // Export structured logs to Aspire dashboard via OpenTelemetry
            if (!string.IsNullOrWhiteSpace(otlpEndpoint))
            {
                configuration.WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = otlpEndpoint;
                    options.Protocol = OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = builder.Environment.ApplicationName
                    };
                });
            }
        });
#pragma warning restore CA1305 // Specify IFormatProvider
    }
}
