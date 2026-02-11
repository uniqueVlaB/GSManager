namespace GSManager.Core.Exceptions.Auth;

public class InvalidCredentialsException : GSManagerUnauthorizedException
{
    public InvalidCredentialsException()
        : base("Invalid email or password.")
    {
    }

    public InvalidCredentialsException(string message)
        : base(message)
    {
    }
}
