namespace GSManager.Core.Exceptions.Auth;

public class InvalidUserRequestException : GSManagerInvalidRequestException
{
    public InvalidUserRequestException()
        : base("The provided user request is invalid.")
    {
    }

    public InvalidUserRequestException(string message)
        : base(message)
    {
    }
}
