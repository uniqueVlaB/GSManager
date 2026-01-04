using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GSManager.API.ExceptionHandlers;

internal sealed class DatabaseExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<DatabaseExceptionHandler> logger) : IExceptionHandler
{
    private const string ConflictTitle = "Conflict";
    private const string BadRequestTitle = "Invalid Request";

    // SQL Server well-known error codes
    private const int SqlServerUniqueConstraintViolation1 = 2601;
    private const int SqlServerUniqueConstraintViolation2 = 2627;
    private const int SqlServerForeignKeyViolation = 547;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Contract of IExceptionHandler: return false if we don't handle
        if (httpContext is null || exception is null)
        {
            return false;
        }

        var dbException = FindDbUpdateException(exception);
        if (dbException is null)
        {
            return false;
        }

        var error = MapDatabaseErrorOrNull(dbException);
        if (error is null)
        {
            // Not a known/handled DB error; let the next handler process it.
            return false;
        }

        LogDatabaseError(dbException, error.Value);

        httpContext.Response.StatusCode = error.Value.StatusCode;

        var problem = CreateProblemDetails(dbException, error.Value);
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = dbException,
            ProblemDetails = problem,
        });
    }

    private static DbUpdateException? FindDbUpdateException(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException!)
        {
            if (current is DbUpdateException db)
            {
                return db;
            }
        }

        return null;
    }

    private static DbError? MapDatabaseErrorOrNull(DbUpdateException exception)
    {
        var providerCode = TryGetProviderErrorCode(exception.InnerException);

        if (IsUniqueViolation(providerCode, exception))
        {
            return new DbError(
                StatusCodes.Status409Conflict,
                ConflictTitle,
                $"A resource with the same unique value already exists. {GetUniqueViolationProperty(exception)}");
        }

        if (IsForeignKeyViolation(providerCode, exception))
        {
            return new DbError(
                StatusCodes.Status400BadRequest,
                BadRequestTitle,
                "The operation violates a foreign key constraint. Probably FK references to a record that does not exist.");
        }

        return null;
    }

    private static string GetUniqueViolationProperty(DbUpdateException exception)
    {
        var indexStart = exception.InnerException?.Message.IndexOf("IX", StringComparison.InvariantCultureIgnoreCase);
        var indexEnd = exception.InnerException?.Message.IndexOf('\'', indexStart.GetValueOrDefault());

        if (indexStart.HasValue && indexEnd.HasValue && indexStart.Value >= 0 && indexEnd.Value > indexStart.Value)
        {
            var indexName = exception.InnerException!.Message[indexStart.Value..indexEnd.Value];
            return $"Index: {indexName}.";
        }
        return string.Empty;
    }

    private static bool IsUniqueViolation(int? providerCode, Exception exception)
    {
        return providerCode is SqlServerUniqueConstraintViolation1 or SqlServerUniqueConstraintViolation2
        || MessageContainsAny(exception, "duplicate key", "UNIQUE", "UNIQUE KEY", "unique index");
    }

    private static bool IsForeignKeyViolation(int? providerCode, Exception exception)
    {
        return providerCode is SqlServerForeignKeyViolation
        || MessageContainsAny(exception, "FOREIGN KEY constraint");
    }

    private static int? TryGetProviderErrorCode(Exception? providerException)
    {
        if (providerException is null)
        {
            return null;
        }

        // Many providers (e.g., SqlException) expose an integer property named 'Number'.
        var numberProp = providerException.GetType().GetProperty("Number");
        return numberProp?.GetValue(providerException) is int number ? number : null;
    }

    private static bool MessageContainsAny(Exception exception, params string[] fragments)
    {
        var message = exception.InnerException?.Message ?? exception.Message ?? string.Empty;

        foreach (var fragment in fragments)
        {
            if (message.Contains(fragment, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private void LogDatabaseError(DbUpdateException exception, DbError error)
    {
        if (error.StatusCode is StatusCodes.Status400BadRequest or StatusCodes.Status409Conflict)
        {
            logger.LogWarning(exception, "Database constraint violation handled: {Title}", error.Title);
        }
        else
        {
            logger.LogError(exception, "Database exception handled: {Title}", error.Title);
        }
    }

    private static ProblemDetails CreateProblemDetails(DbUpdateException exception, DbError error)
    {
        return new()
        {
            Type = exception.GetType().Name,
            Title = error.Title,
            Detail = error.Detail,
            Status = error.StatusCode,
        };
    }

    private readonly record struct DbError(int StatusCode, string Title, string Detail);
}
