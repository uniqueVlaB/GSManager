namespace GSManager.Core.Exceptions.Auth;

public class InvalidPermissionException : GSManagerInvalidRequestException
{
    public InvalidPermissionException()
        : base("The provided permission is invalid.")
    {
    }

    public InvalidPermissionException(string permission)
        : base($"The provided permission '{permission}' is invalid.")
    {
    }
}
