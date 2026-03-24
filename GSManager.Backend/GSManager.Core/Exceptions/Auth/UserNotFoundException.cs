using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Auth;

[ExcludeFromCodeCoverage]
public class UserNotFoundException : GSManagerNotFoundException
{
    public UserNotFoundException(Guid id)
        : base($"User with id '{id}' not found.")
    {
    }

    public UserNotFoundException()
        : base("User not found.")
    {
    }
}
