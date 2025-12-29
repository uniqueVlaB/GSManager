using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class RoleFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (filter.RoleId is null)
        {
            return query;
        }

        return query.Where(m => m.RoleId == filter.RoleId);
    }
}
