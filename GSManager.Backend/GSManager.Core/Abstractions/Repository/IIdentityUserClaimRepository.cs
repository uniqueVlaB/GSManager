using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository;

public interface IIdentityUserClaimRepository : IRepository<IdentityUserClaim<Guid>>
{
}
