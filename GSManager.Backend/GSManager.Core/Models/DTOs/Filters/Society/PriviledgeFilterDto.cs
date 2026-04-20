namespace GSManager.Core.Models.DTOs.Filters.Society;

public class PriviledgeFilterDto
{
    public ICollection<Guid>? Ids { get; init; }
    public ICollection<string>? Names { get; init; }
}

