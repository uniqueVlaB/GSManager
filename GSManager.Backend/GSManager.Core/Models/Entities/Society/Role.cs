namespace GSManager.Core.Models.Entities.Society;

public class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? PriviledgeId { get; set; }
    public Priviledge? Priviledge { get; set; }
}
