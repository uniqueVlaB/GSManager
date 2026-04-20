using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters.Electricity;

namespace GSManager.Core.Filters.ElectricityMeter;

public class OwnerIdFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (filter.OwnerIds is null || filter.OwnerIds.Count == 0)
        {
            return query;
        }

        return query.Where(em => em.OwnerId != null && filter.OwnerIds.Contains(em.OwnerId.Value));
    }
}
