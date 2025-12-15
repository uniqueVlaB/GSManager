using FluentValidation;
using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Abstractions.Services;

public interface IMemberService
{
    Task<ICollection<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken);

    Task<MemberDto> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);

    Task<MemberDto> AddMemberAsync(MemberDto memberDto, CancellationToken cancellationToken);
}
