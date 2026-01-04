namespace GSManager.Core.Models.DTOs;

public class PlotFilterDto
{
    public ICollection<Guid>? Ids { get; init; }
    public ICollection<string>? Numbers { get; init; }
    public ICollection<Guid>? OwnerIds { get; init; }
    public ICollection<Guid>? PriviledgeIds { get; init; }
    public ICollection<string>? CadastreNumbers { get; init; }
}
