namespace GSManager.Core.Models.Entities.Electricity;

public class ElectricityReading
{
    public Guid Id { get; set; }
    public Guid MeterId { get; set; }
    public DateTime ReadingDate { get; set; }
    public decimal ReadingDay { get; set; }
    public decimal? ReadingNight { get; set; }
    public string? Notes { get; set; }

    // Navigation property
    public ElectricityMeter? Meter { get; set; }
}
