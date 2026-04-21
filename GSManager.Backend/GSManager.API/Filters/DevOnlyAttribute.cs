using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GSManager.API.Filters;

/// <summary>
/// Restricts an endpoint to the Development environment only.
/// Returns 404 in all other environments.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class DevOnlyAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();

        if (!env.IsDevelopment())
        {
            context.Result = new NotFoundResult();
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context) { }
}
