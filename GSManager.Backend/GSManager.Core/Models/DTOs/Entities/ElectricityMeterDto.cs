namespace GSManager.Core.Models.DTOs.Entities;

public class ElectricityMeterDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string? Location { get; set; }
    public DateTime InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public string? Notes { get; set; }
    public Guid PlotId { get; set; }
    public Guid? OwnerId { get; set; }
}
