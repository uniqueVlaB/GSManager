using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository;

public interface IRoleRepository : IRepository<Role>
{
    void Update(Role role);
}
