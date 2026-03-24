using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository;

public interface IIdentityRoleClaimsRepository : IRepository<IdentityRoleClaim<Guid>>
{
    void Update(IdentityRoleClaim<Guid> roleClaim);
}
