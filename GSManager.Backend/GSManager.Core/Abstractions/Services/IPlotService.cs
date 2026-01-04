using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Abstractions.Services;

public interface IPlotService
{
    Task<ICollection<PlotDto>> GetAllPlotsAsync(CancellationToken cancellationToken);
    Task<ICollection<PlotDto>> GetFilteredPlotsAsync(PlotFilterDto filter, CancellationToken cancellationToken);
    Task<PlotDto> GetPlotByIdAsync(Guid plotId, CancellationToken cancellationToken);
    Task<PlotDto> AddPlotAsync(PlotDto plotDto, CancellationToken cancellationToken);
    Task<PlotDto> UpdatePlotAsync(Guid plotId, PlotDto plotDto, CancellationToken cancellationToken);
    Task DeletePlotAsync(Guid plotId, CancellationToken cancellationToken);
}
