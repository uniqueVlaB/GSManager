using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerForbiddenException : GSManagerException
{
    public GSManagerForbiddenException()
        : base("Access to this resourse is restricted.")
    {
    }

    public GSManagerForbiddenException(string message)
        : base(message)
    {
    }

    public GSManagerForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
