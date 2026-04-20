using Microsoft.AspNetCore.Identity;

namespace GSManager.Core.Abstractions.Repository.Auth;

public interface IIdentityUserClaimRepository : IRepository<IdentityUserClaim<Guid>>
{
}

