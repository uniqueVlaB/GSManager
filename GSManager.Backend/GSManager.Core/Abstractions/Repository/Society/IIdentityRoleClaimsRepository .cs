using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IIdentityRoleClaimsRepository : IRepository<IdentityRoleClaim<Guid>>
{
    void Update(IdentityRoleClaim<Guid> roleClaim);
}

