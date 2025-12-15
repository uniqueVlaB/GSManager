using FluentValidation;
using GSManager.API;
using GSManager.Core;
using GSManager.Infrastructure.SQL;
using GSManager.Infrastructure.SQL.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

await app.RunAsync().ConfigureAwait(false);
