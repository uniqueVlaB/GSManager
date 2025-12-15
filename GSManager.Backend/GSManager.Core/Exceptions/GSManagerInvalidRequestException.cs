using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerInvalidRequestException : GSManagerException
{
    public GSManagerInvalidRequestException()
        : base("Invalid request.")
    {
    }

    public GSManagerInvalidRequestException(string message)
        : base(message)
    {
    }

    public GSManagerInvalidRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
