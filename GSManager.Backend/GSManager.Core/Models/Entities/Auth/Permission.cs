namespace GSManager.Core.Models.Entities.Auth;

public class Permission
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Role> Roles { get; set; } = [];
    public ICollection<User> Users { get; set; } = [];
}
