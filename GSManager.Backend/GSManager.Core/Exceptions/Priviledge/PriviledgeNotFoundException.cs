using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Priviledge;

[ExcludeFromCodeCoverage]
public class PriviledgeNotFoundException : GSManagerNotFoundException
{
    public PriviledgeNotFoundException(Guid id)
        : base($"Priviledge with id '{id}' not found.")
    {
    }

    public PriviledgeNotFoundException()
        : base("Priviledge not found.")
    {
    }
}
