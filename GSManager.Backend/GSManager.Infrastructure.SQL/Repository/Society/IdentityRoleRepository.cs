using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.AspNetCore.Identity;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class IdentityRoleRepository(ApplicationDbContext db) : Repository<IdentityRole<Guid>>(db), IIdentityRoleRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(IdentityRole<Guid> role)
    {
        _db.Update(role);
    }
}

