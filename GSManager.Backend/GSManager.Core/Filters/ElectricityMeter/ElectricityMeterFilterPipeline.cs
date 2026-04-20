using GSManager.Core.Models.DTOs.Filters.Electricity;

namespace GSManager.Core.Filters.ElectricityMeter;

public static class ElectricityMeterFilterPipeline
{
    public static FilterPipeline<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto> Create()
    {
        return new FilterPipeline<Models.Entities.Electricity.ElectricityMeter, ElectricityMeterFilterDto>()
            .AddFilter(new SearchQueryFilter())
            .AddFilter(new MeterIdFilter())
            .AddFilter(new PlotIdFilter())
            .AddFilter(new OwnerIdFilter())
            .AddFilter(new InstallationDateFilter())
            .AddFilter(new LastMaintenanceDateFilter());
    }
}
