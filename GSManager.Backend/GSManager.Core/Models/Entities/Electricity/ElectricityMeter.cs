using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Models.Entities.Electricity;

public class ElectricityMeter
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string SerialNumber { get; set; }
    public string? Location { get; set; }
    public DateTime InstallationDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public string? Notes { get; set; }
    public Guid PlotId { get; set; }
    public Guid? OwnerId { get; set; }

    // Navigation properties
    public Plot? Plot { get; set; }
    public Member? Owner { get; set; }
    public ICollection<ElectricityReading>? Readings { get; init; }
}
