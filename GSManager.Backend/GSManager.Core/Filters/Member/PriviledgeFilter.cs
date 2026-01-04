using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class PriviledgeFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (filter.PriviledgeIds is null || filter.PriviledgeIds.Count <= 0)
        {
            return query;
        }

        return query.Where(m => m.PriviledgeId.HasValue && filter.PriviledgeIds.Contains(m.PriviledgeId.Value));
    }
}
