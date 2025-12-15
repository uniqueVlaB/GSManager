using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository;

public class PriviledgeRepository(ApplicationDbContext db) : Repository<Priviledge>(db), IPriviledgeRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Priviledge priviledge)
    {
        _db.Update(priviledge);
    }
}
