namespace GSManager.Core.Exceptions.Auth;

public class EmailConfirmationFailedException : GSManagerInvalidRequestException
{
    public EmailConfirmationFailedException()
        : base("Email confirmation failed.")
    {
    }

    public EmailConfirmationFailedException(string message)
        : base(message)
    {
    }
}
