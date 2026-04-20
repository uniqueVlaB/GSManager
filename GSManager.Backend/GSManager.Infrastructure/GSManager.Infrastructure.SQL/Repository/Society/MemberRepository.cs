using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class MemberRepository(ApplicationDbContext db) : Repository<Member>(db), IMemberRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Member member)
    {
        _db.Update(member);
    }
}

