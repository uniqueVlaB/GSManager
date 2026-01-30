using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Plot;

public class NumberFilter : IFilter<Models.Entities.Society.Plot, PlotFilterDto>
{
    public IQueryable<Models.Entities.Society.Plot> Apply(
        IQueryable<Models.Entities.Society.Plot> query,
        PlotFilterDto filter)
    {
        if (filter.Numbers is null || filter.Numbers.Count <= 0)
        {
            return query;
        }

        return query.Where(p => p.Number != null && filter.Numbers.Contains(p.Number));
    }
}
