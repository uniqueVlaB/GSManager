using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IIdentityRoleRepository : IRepository<IdentityRole<Guid>>
{
    void Update(IdentityRole<Guid> role);
}

