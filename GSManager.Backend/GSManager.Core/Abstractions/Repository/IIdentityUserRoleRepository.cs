using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository;

public interface IIdentityUserRoleRepository : IRepository<IdentityUserRole<Guid>>
{
}
