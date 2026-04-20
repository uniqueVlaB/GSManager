using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters.Electricity;

namespace GSManager.Core.Filters.ElectricityMeter;

public class PlotIdFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (filter.PlotIds is null || filter.PlotIds.Count == 0)
        {
            return query;
        }

        return query.Where(em => filter.PlotIds.Contains(em.PlotId));
    }
}
