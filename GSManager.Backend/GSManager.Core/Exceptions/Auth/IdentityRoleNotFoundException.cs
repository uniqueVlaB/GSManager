using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Auth;

[ExcludeFromCodeCoverage]
public class IdentityRoleNotFoundException : GSManagerNotFoundException
{
    public IdentityRoleNotFoundException(Guid id)
        : base($"Identity role with id '{id}' not found.")
    {
    }

    public IdentityRoleNotFoundException(string name)
        : base($"Identity role with name '{name}' not found.")
    {
    }

    public IdentityRoleNotFoundException(IEnumerable<Guid> ids)
        : base($"Identity roles with ids '{string.Join(", ", ids)}' not found.")
    {
    }

    public IdentityRoleNotFoundException()
        : base("Identity role not found.")
    {
    }
}
