using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Priviledge;

public class IdFilter : IFilter<Models.Entities.Society.Priviledge, PriviledgeFilterDto>
{
    public IQueryable<Models.Entities.Society.Priviledge> Apply(
        IQueryable<Models.Entities.Society.Priviledge> query,
        PriviledgeFilterDto filter)
    {
        if (filter.Ids is null || filter.Ids.Count <= 0)
        {
            return query;
        }

        return query.Where(p => filter.Ids.Contains(p.Id));
    }
}
