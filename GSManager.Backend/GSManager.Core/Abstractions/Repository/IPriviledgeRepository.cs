using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository;

public interface IPriviledgeRepository : IRepository<Priviledge>
{
    void Update(Priviledge priviledge);
}
