using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class PlotFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (filter.PlotId is null)
        {
            return query;
        }

        return query.Where(m => m.Plots != null && m.Plots.Any(p => p.Id == filter.PlotId));
    }
}
