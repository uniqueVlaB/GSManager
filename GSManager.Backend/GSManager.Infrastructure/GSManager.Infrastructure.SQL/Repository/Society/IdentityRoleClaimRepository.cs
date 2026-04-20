using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.AspNetCore.Identity;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class IdentityRoleClaimRepository(ApplicationDbContext db) : Repository<IdentityRoleClaim<Guid>>(db), IIdentityRoleClaimsRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(IdentityRoleClaim<Guid> roleClaim)
    {
        _db.Update(roleClaim);
    }
}

