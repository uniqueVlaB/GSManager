using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Auth;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.AspNetCore.Identity;

namespace GSManager.Infrastructure.SQL.Repository.Auth;

public class IdentityUserClaimRepository(ApplicationDbContext db) : Repository<IdentityUserClaim<Guid>>(db), IIdentityUserClaimRepository
{
}

