using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class EmailFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Email))
        {
            return query;
        }

        return query.Where(m => m.Email != null && m.Email.Contains(filter.Email));
    }
}
