using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services;

public interface IPlotService
{
    Task<PagedResultDto<PlotDto>> GetPlotsAsync(PlotFilterDto filter, PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<ICollection<SelectListItemDto>> GetPlotSelectListAsync(CancellationToken cancellationToken);
    Task<PlotDto> GetPlotByIdAsync(Guid plotId, CancellationToken cancellationToken);
    Task<PlotDto> AddPlotAsync(PlotDto plotDto, CancellationToken cancellationToken);
    Task<PlotDto> UpdatePlotAsync(Guid plotId, PlotDto plotDto, CancellationToken cancellationToken);
    Task DeletePlotAsync(Guid plotId, CancellationToken cancellationToken);
}
