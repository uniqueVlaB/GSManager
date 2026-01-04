using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Plot;

public class PlotPriviledgeFilter : IFilter<Models.Entities.Society.Plot, PlotFilterDto>
{
    public IQueryable<Models.Entities.Society.Plot> Apply(
        IQueryable<Models.Entities.Society.Plot> query,
        PlotFilterDto filter)
    {
        if (filter.PriviledgeIds is null || filter.PriviledgeIds.Count <= 0)
        {
            return query;
        }

        return query.Where(p => p.PriviledgeId != null && filter.PriviledgeIds.Contains(p.PriviledgeId.Value));
    }
}
