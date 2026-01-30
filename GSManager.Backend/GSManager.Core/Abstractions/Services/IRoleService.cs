using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services;

public interface IRoleService
{
    Task<PagedResultDto<RoleDto>> GetRolesAsync(RoleFilterDto filter, PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<ICollection<SelectListItemDto>> GetRoleSelectListAsync(CancellationToken cancellationToken);
    Task<RoleDto> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task<RoleDto> AddRoleAsync(RoleDto roleDto, CancellationToken cancellationToken);
    Task<RoleDto> UpdateRoleAsync(Guid roleId, RoleDto roleDto, CancellationToken cancellationToken);
    Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken);
}
