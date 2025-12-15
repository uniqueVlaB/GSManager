using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.GSManager_API>("GSManagerAPI");

var name = "scalar-ui-docs";
var displayName = "Scalar UI Documentation";
var openApiUiPath = "scalar/v1";
api.WithCommand(
    name,
    displayName,
    executeCommand: async _ =>
    {
        try
        {
            var endpoint = api.GetEndpoint("https");

            var url = $"{endpoint.Url}/{openApiUiPath}";

            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

            return new ExecuteCommandResult { Success = true };
        }
        catch (Exception e)
        {
            return new ExecuteCommandResult { Success = false, ErrorMessage = e.ToString() };
        }
    },
    commandOptions: new CommandOptions
    {
        UpdateState = context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy
            ? ResourceCommandState.Enabled
            : ResourceCommandState.Disabled,
        IconName = "Document",
        IconVariant = IconVariant.Filled
    });

builder.AddNpmApp("GSManagerAngular", "../GSManager.Angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(targetPort: 4200)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation();

await builder.Build().RunAsync().ConfigureAwait(false);
