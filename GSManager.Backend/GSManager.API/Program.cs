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

var app = builder.Build();

app.ConfigureCustomMiddlewares();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.ApplyDatabaseMigrations();

await app.RunAsync();
