using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerException : Exception
{
    public GSManagerException()
    {
    }

    public GSManagerException(string message)
        : base(message)
    {
    }

    public GSManagerException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}