using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IIdentityUserRoleRepository : IRepository<IdentityUserRole<Guid>>
{
}

