using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class RoleIdFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (filter.RoleIds is null || filter.RoleIds.Count <= 0)
        {
            return query;
        }

        return query.Where(m => m.RoleId.HasValue && filter.RoleIds.Contains(m.RoleId.Value));
    }
}
