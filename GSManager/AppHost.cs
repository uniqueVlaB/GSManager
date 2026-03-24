using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = DistributedApplication.CreateBuilder(args);

#if DEBUG
var api = builder.AddProject<Projects.GSManager_API>("gsmanager-api");

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
#else
var postgres = builder.AddPostgres("postgres");
var gsmanagerdb = postgres.AddDatabase("gsmanagerdb");

var api = builder.AddProject<Projects.GSManager_API>("gsmanager-api")
    .WithReference(gsmanagerdb)
    .WaitFor(gsmanagerdb);
#endif

builder.AddNpmApp("GSManagerAngular", "../GSManager.Angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(name: "angular",targetPort: 4200, port: 4300)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation();

await builder.Build().RunAsync();
