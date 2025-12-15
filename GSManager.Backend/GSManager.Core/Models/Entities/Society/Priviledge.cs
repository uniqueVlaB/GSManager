namespace GSManager.Core.Models.Entities.Society;

public class Priviledge
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
