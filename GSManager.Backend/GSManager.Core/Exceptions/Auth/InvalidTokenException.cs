namespace GSManager.Core.Exceptions.Auth;

public class InvalidTokenException : GSManagerException
{
    public InvalidTokenException()
        : base("The provided token is invalid or expired.")
    {
    }
}
