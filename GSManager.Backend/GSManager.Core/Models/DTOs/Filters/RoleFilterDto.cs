namespace GSManager.Core.Models.DTOs.Filters;

public class RoleFilterDto
{
    public ICollection<Guid>? Ids { get; init; }
    public string? Name { get; init; }
    public string? SearchQuery { get; init; }
    public ICollection<Guid>? PriviledgeIds { get; init; }
}
