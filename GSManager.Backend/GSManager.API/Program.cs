using FluentValidation;
using GSManager.API;
using GSManager.API.Config;
using GSManager.API.Middleware;
using GSManager.Core;
using GSManager.Infrastructure.SQL;
using GSManager.Infrastructure.SQL.Database;
using GSManager.Infrastructure.SQL.Options;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using AspireServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

SerilogConfigurator.Configure(builder);

builder.Services.AddCoreServices();
builder.Services.AddApiServices();

var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.FullName?.StartsWith("GSManager.Core,", StringComparison.OrdinalIgnoreCase) == true)
    .ToArray();

builder.Services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection("SqlServerDatabase"));

builder.Services.AddSqlInfrastructureServices();

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

await app.RunAsync();
