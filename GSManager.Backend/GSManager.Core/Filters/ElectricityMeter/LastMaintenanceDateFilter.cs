using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters.Electricity;

namespace GSManager.Core.Filters.ElectricityMeter;

public class LastMaintenanceDateFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (filter.LastMaintenanceDateFrom.HasValue)
        {
            query = query.Where(em => em.LastMaintenanceDate != null && em.LastMaintenanceDate >= filter.LastMaintenanceDateFrom.Value);
        }

        if (filter.LastMaintenanceDateTo.HasValue)
        {
            query = query.Where(em => em.LastMaintenanceDate != null && em.LastMaintenanceDate <= filter.LastMaintenanceDateTo.Value);
        }

        return query;
    }
}
