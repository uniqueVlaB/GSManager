using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Role;

public class PriviledgeFilter : IFilter<Models.Entities.Society.Role, RoleFilterDto>
{
    public IQueryable<Models.Entities.Society.Role> Apply(
        IQueryable<Models.Entities.Society.Role> query,
        RoleFilterDto filter)
    {
        if (filter.PriviledgeIds is null || filter.PriviledgeIds.Count < 1)
        {
            return query;
        }

        return query.Where(r => r.PriviledgeId.HasValue && filter.PriviledgeIds.Contains(r.PriviledgeId.Value));
    }
}
