using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Abstractions.Services;

public interface IPriviledgeService
{
    Task<ICollection<PriviledgeDto>> GetAllPriviledgesAsync(CancellationToken cancellationToken);
    Task<ICollection<PriviledgeDto>> GetFilteredPriviledgesAsync(PriviledgeFilterDto filter, CancellationToken cancellationToken);
    Task<ICollection<SelectListItemDto>> GetPriviledgeSelectListAsync(CancellationToken cancellationToken);
    Task<PriviledgeDto> GetPriviledgeByIdAsync(Guid priviledgeId, CancellationToken cancellationToken);
    Task<PriviledgeDto> AddPriviledgeAsync(PriviledgeDto priviledgeDto, CancellationToken cancellationToken);
    Task<PriviledgeDto> UpdatePriviledgeAsync(Guid priviledgeId, PriviledgeDto priviledgeDto, CancellationToken cancellationToken);
    Task DeletePriviledgeAsync(Guid priviledgeId, CancellationToken cancellationToken);
}
