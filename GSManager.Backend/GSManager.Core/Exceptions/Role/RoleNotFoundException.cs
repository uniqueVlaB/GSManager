using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Role;

[ExcludeFromCodeCoverage]
public class RoleNotFoundException : GSManagerNotFoundException
{
    public RoleNotFoundException(Guid id)
        : base($"Role with id '{id}' not found.")
    {
    }

    public RoleNotFoundException()
        : base("Role not found.")
    {
    }
}
