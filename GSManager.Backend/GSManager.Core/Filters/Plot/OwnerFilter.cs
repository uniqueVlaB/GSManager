using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Plot;

public class OwnerFilter : IFilter<Models.Entities.Society.Plot, PlotFilterDto>
{
    public IQueryable<Models.Entities.Society.Plot> Apply(
        IQueryable<Models.Entities.Society.Plot> query,
        PlotFilterDto filter)
    {
        if (filter.OwnerIds is null || filter.OwnerIds.Count == 0)
        {
            return query;
        }

        return query.Where(p => p.OwnerId.HasValue && filter.OwnerIds.Contains(p.OwnerId.Value));
    }
}
