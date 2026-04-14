namespace GSManager.Core.Models.DTOs.Filters;

public class ElectricityMeterFilterDto
{
    public ICollection<Guid>? PlotIds { get; init; }
    public ICollection<Guid>? OwnerIds { get; init; }
    public ICollection<Guid>? MeterIds { get; init; }
    public DateTime? InstallationDateFrom { get; init; }
    public DateTime? InstallationDateTo { get; init; }
    public DateTime? LastMaintenanceDateFrom { get; init; }
    public DateTime? LastMaintenanceDateTo { get; init; }
    public string? SearchQuery { get; init; }
}
