using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions;

[ExcludeFromCodeCoverage]
public class GSManagerValueIsRequiredException : GSManagerException
{
    public GSManagerValueIsRequiredException()
        : base("A required value is null.")
    {
    }

    public GSManagerValueIsRequiredException(string message)
        : base(message)
    {
    }

    public GSManagerValueIsRequiredException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
