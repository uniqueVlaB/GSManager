namespace GSManager.Core.Models.DTOs.Entities;

public class RoleDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? PriviledgeId { get; set; }
}
