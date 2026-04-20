using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IRoleRepository : IRepository<Role>
{
    void Update(Role role);
}

