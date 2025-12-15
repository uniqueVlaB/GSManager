namespace GSManager.Core.Models.DTOs;

public class MemberDto
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? PriviledgeId { get; set; }
    public IList<Guid>? PlotIds { get; init; }
}

