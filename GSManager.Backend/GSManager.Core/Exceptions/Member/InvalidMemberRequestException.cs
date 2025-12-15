using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Member;

[ExcludeFromCodeCoverage]
public class InvalidMemberRequestException : GSManagerInvalidRequestException
{
    public InvalidMemberRequestException()
        : base("Invalid member request.")
    {
    }

    public InvalidMemberRequestException(string message)
        : base(message)
    {
    }
}
