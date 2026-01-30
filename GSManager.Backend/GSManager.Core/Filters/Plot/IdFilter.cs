using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Plot;

public class IdFilter : IFilter<Models.Entities.Society.Plot, PlotFilterDto>
{
    public IQueryable<Models.Entities.Society.Plot> Apply(
        IQueryable<Models.Entities.Society.Plot> query,
        PlotFilterDto filter)
    {
        if (filter.Ids is null || filter.Ids.Count == 0)
        {
            return query;
        }

        return query.Where(p => filter.Ids.Contains(p.Id));
    }
}
