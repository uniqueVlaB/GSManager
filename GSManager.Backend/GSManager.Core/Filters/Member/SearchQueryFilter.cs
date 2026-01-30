using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Member;

public class SearchQueryFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            return query;
        }

        return query.Where(m =>
            (m.Email != null && m.Email.Contains(filter.SearchQuery)) ||
            (m.FirstName != null && m.FirstName.Contains(filter.SearchQuery)) ||
            (m.LastName != null && m.LastName.Contains(filter.SearchQuery)) ||
            (m.PhoneNumber != null && m.PhoneNumber.Contains(filter.SearchQuery))||
            (m.MiddleName != null && m.MiddleName.Contains(filter.SearchQuery))
        );
    }
}
