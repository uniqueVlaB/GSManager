namespace GSManager.Core.Models.Entities.Society;

public class Member
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public List<Plot>? Plots { get; init; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid? RoleId { get; set; }
    public Role? Role { get; set; }
    public Guid? PriviledgeId { get; set; }
    public Priviledge? Priviledge { get; set; }
}
