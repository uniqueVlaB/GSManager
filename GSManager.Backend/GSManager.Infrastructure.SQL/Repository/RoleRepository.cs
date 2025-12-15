using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository;

public class RoleRepository(ApplicationDbContext db) : Repository<Role>(db), IRoleRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Role role)
    {
        _db.Update(role);
    }
}
