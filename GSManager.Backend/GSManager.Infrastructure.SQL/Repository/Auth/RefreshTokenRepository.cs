using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Auth;
using GSManager.Core.Models.Entities.Auth;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Auth;

public class RefreshTokenRepository(ApplicationDbContext db) : Repository<RefreshToken>(db), IRefreshTokenRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(RefreshToken refreshToken)
    {
        _db.Update(refreshToken);
    }
}
