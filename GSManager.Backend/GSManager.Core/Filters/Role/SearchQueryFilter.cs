using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Role;

public class SearchQueryFilter : IFilter<Models.Entities.Society.Role, RoleFilterDto>
{
    public IQueryable<Models.Entities.Society.Role> Apply(
        IQueryable<Models.Entities.Society.Role> query,
        RoleFilterDto filter)
    {
        if (string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            return query;
        }

        return query.Where(r =>
            r.Name.Contains(filter.SearchQuery) ||
            (r.Description != null && r.Description.Contains(filter.SearchQuery))
        );
    }
}
