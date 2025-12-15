namespace GSManager.Core.Models.Entities.Society;

public class Plot
{
    public Guid Id { get; set; }
    public required string Number { get; set; }
    public string? Description { get; set; }
    public Guid? OwnerId { get; set; }
    public Member? Owner { get; set; }
    public float? Square { get; set; }
    public Guid? PriviledgeId { get; set; }
    public Priviledge? Priviledge { get; set; }
    public string? CadastreNumber { get; set; } = string.Empty;
}
