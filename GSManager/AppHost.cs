using System.Diagnostics;
using GSManager;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = DistributedApplication.CreateBuilder(args);

// ── Infrastructure ────────────────────────────────────────────────────────────

var messaging = builder.AddRabbitMQ("rabbit-mq")
    .WithManagementPlugin();

// ── Parameters (values stored in AppHost user secrets) ────────────────────────

var jwt = builder.AddJwtParameters();
var mailer = builder.AddMailerParameters();

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume("gsmanager-postgres-data")
    .WithPgAdmin()
    .WithHostPort(5432);
var db = postgres.AddDatabase("gsmanager-db");

// ── Services ──────────────────────────────────────────────────────────────────

var mailerService = builder.AddProject<Projects.GSManager_Mailer>("gsmanager-mailer")
    .WithReference(messaging)
    .WaitFor(messaging)
    .WithEnvironment("MailerSettings__Server", mailer.Server)
    .WithEnvironment("MailerSettings__Port", mailer.Port)
    .WithEnvironment("MailerSettings__SenderName", mailer.SenderName)
    .WithEnvironment("MailerSettings__SenderEmail", mailer.SenderEmail)
    .WithEnvironment("MailerSettings__Username", mailer.Username)
    .WithEnvironment("MailerSettings__Password", mailer.Password)
    .WithEnvironment("MailerSettings__FrontendBaseUrl", mailer.FrontendBaseUrl);

var api = builder.AddProject<Projects.GSManager_API>("gsmanager-api")
    .WithReference(messaging)
    .WaitFor(messaging)
    .WithReference(db)
    .WaitFor(db)
    .WithEnvironment("Jwt__SecretKey", jwt.SecretKey)
    .WithEnvironment("Jwt__Issuer", jwt.Issuer)
    .WithEnvironment("Jwt__Audience", jwt.Audience)
    .WithEnvironment("Jwt__ExpirationInMinutes", jwt.ExpirationInMinutes)
    .WithEnvironment("Jwt__RefreshTokenExpirationInDays", jwt.RefreshTokenExpirationInDays);

#if DEBUG
api.WithCommand(
    name: "scalar-ui-docs",
    displayName: "Scalar UI Documentation",
    executeCommand: async _ =>
    {
        try
        {
            var url = $"{api.GetEndpoint("https").Url}/scalar/v1";
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
#endif

builder.AddNpmApp("GSManagerAngular", "../GSManager.Angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(name: "angular", targetPort: 4200, port: 4300)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation();

await builder.Build().RunAsync();

#if DEBUG
api.WithCommand(
    name: "scalar-ui-docs",
    displayName: "Scalar UI Documentation",
    executeCommand: async _ =>
    {
        try
        {
            var url = $"{api.GetEndpoint("https").Url}/scalar/v1";
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
#endif

builder.AddNpmApp("GSManagerAngular", "../GSManager.Angular")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(name: "angular", targetPort: 4200, port: 4300)
    .WithExternalHttpEndpoints()
    .WithNpmPackageInstallation();

await builder.Build().RunAsync();
