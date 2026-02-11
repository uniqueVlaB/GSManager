namespace GSManager.Core.Exceptions.Auth;

public class InvalidTokenException : GSManagerUnauthorizedException
{
    public InvalidTokenException()
        : base("The provided token is invalid or expired.")
    {
    }

    public InvalidTokenException(string message)
    : base(message)
    {
    }
}
