using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Plot;

public class CadastreNumberFilter : IFilter<Models.Entities.Society.Plot, PlotFilterDto>
{
    public IQueryable<Models.Entities.Society.Plot> Apply(
        IQueryable<Models.Entities.Society.Plot> query,
        PlotFilterDto filter)
    {
        if (filter.CadastreNumbers is null || filter.CadastreNumbers.Count <= 0)
        {
            return query;
        }

        return query.Where(p => p.CadastreNumber != null && filter.CadastreNumbers.Any(cn => p.CadastreNumber.Contains(cn)));
    }
}
