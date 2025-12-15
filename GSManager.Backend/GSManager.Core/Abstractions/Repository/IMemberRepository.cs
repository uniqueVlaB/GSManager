using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository;

public interface IMemberRepository : IRepository<Member>
{
    void Update(Member member);
}
