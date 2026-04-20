namespace GSManager.Core.Models.DTOs.Filters.Electricity;

public class ElectricityMeterFilterDto
{
    public string? SearchQuery { get; init; }
    public ICollection<Guid>? MeterIds { get; init; }
    public ICollection<Guid>? PlotIds { get; init; }
    public ICollection<Guid>? OwnerIds { get; init; }
    public DateTime? InstallationDateFrom { get; init; }
    public DateTime? InstallationDateTo { get; init; }
    public DateTime? LastMaintenanceDateFrom { get; init; }
    public DateTime? LastMaintenanceDateTo { get; init; }
}
