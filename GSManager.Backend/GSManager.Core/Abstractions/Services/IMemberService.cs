using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services;

public interface IMemberService
{
    Task<PagedResultDto<MemberDto>> GetMembersAsync(MemberFilterDto filter, PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<ICollection<SelectListItemDto>> GetMemberSelectListAsync(CancellationToken cancellationToken);
    Task<MemberDto> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken);
    Task<MemberDto> AddMemberAsync(MemberDto memberDto, CancellationToken cancellationToken);
    Task DeleteMemberAsync(Guid memberId, CancellationToken cancellationToken);
    Task<MemberDto> UpdateMemberAsync(Guid memberId, MemberDto memberDto, CancellationToken cancellationToken);
}
