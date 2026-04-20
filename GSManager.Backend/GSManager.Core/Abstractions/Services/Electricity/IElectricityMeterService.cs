using GSManager.Core.Models.DTOs.Entities.Electricity;
using GSManager.Core.Models.DTOs.Filters.Electricity;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services.Electricity;

public interface IElectricityMeterService
{
    Task<PagedResultDto<ElectricityMeterDto>> GetElectricityMetersAsync(ElectricityMeterFilterDto filter, PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<ElectricityMeterDto> GetElectricityMeterByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<ElectricityMeterDto> AddElectricityMeterAsync(ElectricityMeterDto electricityMeterDto, CancellationToken cancellationToken);
}
