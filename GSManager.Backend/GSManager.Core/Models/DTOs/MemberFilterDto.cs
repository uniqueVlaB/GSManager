namespace GSManager.Core.Models.DTOs;

public class MemberFilterDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public Guid? RoleId { get; init; }
    public Guid? PriviledgeId { get; init; }
    public Guid? PlotId { get; init; }
}
