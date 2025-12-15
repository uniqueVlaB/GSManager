using GSManager.Core.Models.DTOs;
using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Mappers;

public static class MemberMapper
{
    public static MemberDto ToDto(Member member)
    {
        return new MemberDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            MiddleName = member.MiddleName,
            LastName = member.LastName,
            PhoneNumber = member.PhoneNumber,
            Email = member.Email,
            RoleId = member.RoleId,
            PriviledgeId = member.PriviledgeId,
            PlotIds = member.Plots?.Select(p => p.Id).ToList()
        };
    }

    public static Member ToEntity(MemberDto memberDto, Role? role = null, Priviledge? priviledge = null, List<Plot>? plots = null)
    {
        return new Member
        {
            Id = memberDto.Id ?? Guid.NewGuid(),
            FirstName = memberDto.FirstName!,
            MiddleName = memberDto.MiddleName,
            LastName = memberDto.LastName!,
            PhoneNumber = memberDto.PhoneNumber,
            Email = memberDto.Email,
            RoleId = memberDto.RoleId,
            PriviledgeId = memberDto.PriviledgeId,
            Role = role,
            Priviledge = priviledge,
            Plots = plots
        };
    }
}
