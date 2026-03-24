using GSManager.Core.Models.Entities.Auth;

namespace GSManager.Core.Abstractions.Repository;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    void Update(RefreshToken refreshToken);
}
