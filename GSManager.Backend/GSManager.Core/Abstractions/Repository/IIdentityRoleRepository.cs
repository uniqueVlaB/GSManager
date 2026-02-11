using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository;

public interface IIdentityRoleRepository : IRepository<IdentityRole<Guid>>
{
    void Update(IdentityRole<Guid> role);
}
