using GSManager.Core.Models.Entities.Accounting;
using GSManager.Core.Models.Entities.Electricity;

namespace GSManager.Core.Models.Entities.Society;

public class Member
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public List<Plot>? Plots { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? PriviledgeId { get; set; }

    // Navigation properties
    public Role? Role { get; set; }
    public Priviledge? Priviledge { get; set; }
    public ICollection<ElectricityMeter>? ElectricityMeters { get; init; }
    public ICollection<Payment>? Payments { get; init; }
}
