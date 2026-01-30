namespace GSManager.Core.Models.DTOs.Entities;

public class MemberDto
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? PriviledgeId { get; set; }
    public IList<Guid>? PlotIds { get; init; }
}

