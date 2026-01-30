using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Mappers;

public static class RoleMapper
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            PriviledgeId = role.PriviledgeId
        };
    }

    public static Role ToEntity(RoleDto roleDto, Priviledge? priviledge = null)
    {
        return new Role
        {
            Id = roleDto.Id ?? Guid.NewGuid(),
            Name = roleDto.Name!,
            Description = roleDto.Description,
            PriviledgeId = roleDto.PriviledgeId,
            Priviledge = priviledge
        };
    }

    public static void UpdateEntity(Role role, RoleDto roleDto, Priviledge? priviledge = null)
    {
        role.Name = roleDto.Name!;
        role.Description = roleDto.Description;
        role.PriviledgeId = roleDto.PriviledgeId;
        role.Priviledge = priviledge;
    }
}
