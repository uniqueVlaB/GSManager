using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.ElectricityMeter;

public class InstallationDateFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (filter.InstallationDateFrom.HasValue)
        {
            query = query.Where(em => em.InstallationDate >= filter.InstallationDateFrom.Value);
        }

        if (filter.InstallationDateTo.HasValue)
        {
            query = query.Where(em => em.InstallationDate <= filter.InstallationDateTo.Value);
        }

        return query;
    }
}
