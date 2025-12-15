using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Priviledge;

[ExcludeFromCodeCoverage]
public class InvalidPriviledgeRequestException : GSManagerInvalidRequestException
{
    public InvalidPriviledgeRequestException()
        : base("Invalid role request.")
    {
    }

    public InvalidPriviledgeRequestException(string message)
        : base(message)
    {
    }
}
