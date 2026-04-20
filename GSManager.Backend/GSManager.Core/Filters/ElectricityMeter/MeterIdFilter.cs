using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters.Electricity;

namespace GSManager.Core.Filters.ElectricityMeter;

public class MeterIdFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (filter.MeterIds is null || filter.MeterIds.Count == 0)
        {
            return query;
        }

        return query.Where(em => filter.MeterIds.Contains(em.Id));
    }
}
