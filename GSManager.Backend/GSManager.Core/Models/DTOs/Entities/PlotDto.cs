namespace GSManager.Core.Models.DTOs.Entities;

public class PlotDto
{
    public Guid? Id { get; set; }
    public string? Number { get; set; }
    public string? Description { get; set; }
    public Guid? OwnerId { get; set; }
    public float? Square { get; set; }
    public Guid? PriviledgeId { get; set; }
    public string? CadastreNumber { get; set; }
}
