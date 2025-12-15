using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Role;

[ExcludeFromCodeCoverage]
public class InvalidRoleRequestException : GSManagerInvalidRequestException
{
    public InvalidRoleRequestException()
        : base("Invalid role request.")
    {
    }

    public InvalidRoleRequestException(string message)
        : base(message)
    {
    }
}
