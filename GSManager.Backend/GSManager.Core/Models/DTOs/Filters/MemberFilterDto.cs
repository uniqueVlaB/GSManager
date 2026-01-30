namespace GSManager.Core.Models.DTOs.Filters;

public class MemberFilterDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public ICollection<Guid>? Ids { get; init; }
    public ICollection<Guid>? RoleIds { get; init; }
    public ICollection<Guid>? PriviledgeIds { get; init; }
    public ICollection<Guid>? PlotIds { get; init; }

    public string? SearchQuery { get; init; }
}
