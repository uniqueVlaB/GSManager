using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Role;

public class IdFilter : IFilter<Models.Entities.Society.Role, RoleFilterDto>
{
    public IQueryable<Models.Entities.Society.Role> Apply(
        IQueryable<Models.Entities.Society.Role> query,
        RoleFilterDto filter)
    {
        if (filter.Ids is null || filter.Ids.Count == 0)
        {
            return query;
        }

        return query.Where(r => filter.Ids.Contains(r.Id));
    }
}
