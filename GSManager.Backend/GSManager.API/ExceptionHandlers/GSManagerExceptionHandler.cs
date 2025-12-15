using GSManager.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.ExceptionHandlers;

internal sealed class GSManagerExceptionHandler(
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private const string UnexpectedErrorUserMessage = "An unexpected error occurred. Please try again later.";
    private const string DefaultUserMessage = "An error occurred while processing your request.";
    private const string NotFoundTitle = "Resource Not Found";
    private const string BadRequestTitle = "Invalid Request";
    private const string ForbiddenTitle = "Forbidden";
    private const string InternalServerErrorTitle = "Server Error";
    private const string DefaultTitle = "Error";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (httpContext is null || exception is null || exception is not GSManagerException)
        {
            return false;
        }

        var statusCode = GetStatusCode(exception);
        var userMessage = GetUserMessage(statusCode, exception);
        var title = GetTitle(statusCode);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = title,
                Detail = userMessage,
                Status = statusCode,
            },
        }).ConfigureAwait(false);
    }

    private static int GetStatusCode(Exception? exception)
    {
        return exception switch
        {
            GSManagerNotFoundException => StatusCodes.Status404NotFound,
            GSManagerInvalidRequestException => StatusCodes.Status400BadRequest,
            GSManagerForbiddenException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    private static string GetUserMessage(int statusCode, Exception ex)
    {
        return statusCode switch
        {
            StatusCodes.Status404NotFound => ex.Message,
            StatusCodes.Status403Forbidden => ex.Message,
            StatusCodes.Status400BadRequest => ex.Message,
            StatusCodes.Status500InternalServerError => UnexpectedErrorUserMessage,
            _ => DefaultUserMessage,
        };
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status404NotFound => NotFoundTitle,
            StatusCodes.Status403Forbidden => ForbiddenTitle,
            StatusCodes.Status400BadRequest => BadRequestTitle,
            StatusCodes.Status500InternalServerError => InternalServerErrorTitle,
            _ => DefaultTitle,
        };
    }

    private static string GetExceptionDetails(Exception ex)
    {
        var details = $"Type: {ex.GetType().FullName}\nMessage: {ex.Message}\nStackTrace: {ex.StackTrace}";
        if (ex.InnerException != null)
        {
            details += $"\nInnerException: {GetExceptionDetails(ex.InnerException)}";
        }

        return details;
    }
}