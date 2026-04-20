using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class RoleRepository(ApplicationDbContext db) : Repository<Role>(db), IRoleRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Role role)
    {
        _db.Update(role);
    }
}

