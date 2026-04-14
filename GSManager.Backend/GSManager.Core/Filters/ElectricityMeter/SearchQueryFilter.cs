using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.ElectricityMeter;

public class SearchQueryFilter : IFilter<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>
{
    public IQueryable<Models.Entities.Electricity.ElectricityMeter> Apply(
        IQueryable<Models.Entities.Electricity.ElectricityMeter> query,
        ElectricityMeterFilterDto filter)
    {
        if (string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            return query;
        }

        var lowerQuery = filter.SearchQuery.ToLower();
        return query.Where(em => em.Name.ToLower().Contains(lowerQuery) || em.SerialNumber.ToLower().Contains(lowerQuery));
    }
}
