using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerNotFoundException : GSManagerException
{
    public GSManagerNotFoundException()
        : base("Requested resource was not found.")
    {
    }

    public GSManagerNotFoundException(string message)
        : base(message)
    {
    }

    public GSManagerNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
