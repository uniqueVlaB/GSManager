using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.ExceptionHandlers;

[ExcludeFromCodeCoverage]
internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService
    /*ILogger<GlobalExceptionHandler> logger*/) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext is null || exception is null)
        {
            return false;
        }

        //logger.LogError(exception, "Unhandled exception occurred");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = "See details in logs",
            },
        }).ConfigureAwait(false);
    }
}