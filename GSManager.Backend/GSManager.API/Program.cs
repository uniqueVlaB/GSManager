using GSManager.API.Config;
using GSManager.Core;
using GSManager.Infrastructure.SQL;
using Scalar.AspNetCore;
using AspireServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.ConfigureSerilog();
builder.ConfigureAuthentication();
builder.ConfigureOptionsPatterns();

builder.Services.AddCoreServices();
builder.Services.AddApiServices();
builder.Services.AddSqlInfrastructureServices();
builder.Services.AddIdentityServices();

var app = builder.Build();

app.ConfigureCustomMiddlewares();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    await app.ApplyDatabaseMigrationsAsync();
    await app.SeedDefaultIdentityAsync();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

await app.RunAsync();
