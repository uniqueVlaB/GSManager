using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IPriviledgeRepository : IRepository<Priviledge>
{
    void Update(Priviledge priviledge);
}

