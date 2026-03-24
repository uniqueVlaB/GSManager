using GSManager.API.Config;
using GSManager.Core;
using GSManager.Infrastructure.SQL;
using GSManager.Infrastructure.Mailer;
using Scalar.AspNetCore;
using AspireServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Infrastructure
builder.AddServiceDefaults();
builder.ConfigureSerilog();

// Services
builder.Services.AddCoreServices();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddSqlInfrastructureServices();
builder.Services.AddMailerInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices();

// Auth
builder.AddAuth();

var app = builder.Build();

// Middleware pipeline
app.UseCustomMiddlewares();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.InitializeDatabaseAsync();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

await app.RunAsync();
