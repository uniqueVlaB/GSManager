using GSManager.Core.Models.DTOs.Entities.Society;
using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Mappers.Society;

public static class PriviledgeMapper
{
    public static PriviledgeDto ToDto(Priviledge priviledge)
    {
        return new PriviledgeDto
        {
            Id = priviledge.Id,
            Name = priviledge.Name,
            Description = priviledge.Description
        };
    }

    public static Priviledge ToEntity(PriviledgeDto priviledgeDto)
    {
        return new Priviledge
        {
            Id = priviledgeDto.Id ?? Guid.NewGuid(),
            Name = priviledgeDto.Name!,
            Description = priviledgeDto.Description
        };
    }
}

