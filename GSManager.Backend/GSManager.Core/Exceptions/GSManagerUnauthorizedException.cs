using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerUnauthorizedException : GSManagerException
{
    public GSManagerUnauthorizedException()
        : base("Unauthorized request.")
    {
    }

    public GSManagerUnauthorizedException(string message)
        : base(message)
    {
    }

    public GSManagerUnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
