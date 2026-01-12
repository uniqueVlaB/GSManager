using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public class PlotIdFilter : IFilter<Models.Entities.Society.Member, MemberFilterDto>
{
    public IQueryable<Models.Entities.Society.Member> Apply(
        IQueryable<Models.Entities.Society.Member> query,
        MemberFilterDto filter)
    {
        if (filter.PlotIds is null || filter.PlotIds.Count <= 0)
        {
            return query;
        }

        return query.Where(m => m.Plots != null && m.Plots.Any(p => filter.PlotIds.Contains(p.Id)));
    }
}
