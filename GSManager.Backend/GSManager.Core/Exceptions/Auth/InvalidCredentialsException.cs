namespace GSManager.Core.Exceptions.Auth;

public class InvalidCredentialsException : GSManagerException
{
    public InvalidCredentialsException()
        : base("Invalid email or password.")
    {
    }
}
