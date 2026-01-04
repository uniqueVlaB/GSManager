namespace GSManager.Core.Models.DTOs;

public class PriviledgeFilterDto
{
    public ICollection<Guid>? Ids { get; init; }
    public ICollection<string>? Names { get; init; }
}
