using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository;

public class MemberRepository(ApplicationDbContext db) : Repository<Member>(db), IMemberRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Member member)
    {
        _db.Update(member);
    }
}
