using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services.Electricity;
using GSManager.Core.Exceptions.Electricity;
using GSManager.Core.Extensions;
using GSManager.Core.Filters.ElectricityMeter;
using GSManager.Core.Mappers.Electricity;
using GSManager.Core.Models.DTOs.Entities.Electricity;
using GSManager.Core.Models.DTOs.Filters.Electricity;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Services.Electricity;

public class ElectricityMeterService(
    IUnitOfWork unitOfWork,
    IValidator<ElectricityMeterDto> validator) : IElectricityMeterService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<ElectricityMeterDto> _validator = validator;

    public async Task<ElectricityMeterDto> AddElectricityMeterAsync(ElectricityMeterDto electricityMeterDto, CancellationToken cancellationToken)
    {
        await ValidateElectricityMeterAsync(electricityMeterDto, cancellationToken);

        var electricityMeter = ElectricityMeterMapper.ToEntity(electricityMeterDto);
        _unitOfWork.ElectricityMeters.Add(electricityMeter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ElectricityMeterMapper.ToDto(electricityMeter);
    }

    public async Task<ElectricityMeterDto> GetElectricityMeterByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var electricityMeter = await _unitOfWork.ElectricityMeters.GetAsync(em => em.Id == id, cancellationToken)
            ?? throw new ElectricityMeterNotFoundException(id);
        return ElectricityMeterMapper.ToDto(electricityMeter);
    }

    public async Task<PagedResultDto<ElectricityMeterDto>> GetElectricityMetersAsync(ElectricityMeterFilterDto filter, PagedRequestDto pagedRequest, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.ElectricityMeters.GetQueryable();

        var pipeline = ElectricityMeterFilterPipeline.Create();
        query = pipeline.Execute(query, filter);

        return await query.ToPagedResultDtoAsync(
            pagedRequest.Page,
            pagedRequest.PageSize,
            ElectricityMeterMapper.ToDto,
            em => em.Name,
            cancellationToken);
    }

    private async Task ValidateElectricityMeterAsync(ElectricityMeterDto electricityMeterDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(electricityMeterDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidElectricityMeterRequestException(validationResult.ToString());
        }
    }
}
