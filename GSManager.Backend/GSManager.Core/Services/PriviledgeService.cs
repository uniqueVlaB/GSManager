using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Exceptions.Priviledge;
using GSManager.Core.Filters.Priviledge;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services;

public class PriviledgeService(
    IUnitOfWork unitOfWork,
    IValidator<PriviledgeDto> validator
    ) : IPriviledgeService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PriviledgeDto> _validator = validator;

    public async Task<ICollection<PriviledgeDto>> GetAllPriviledgesAsync(CancellationToken cancellationToken)
    {
        var priviledges = await _unitOfWork.Priviledges.GetAllAsync(cancellationToken);
        return priviledges.Select(PriviledgeMapper.ToDto).ToList();
    }

    public async Task<ICollection<PriviledgeDto>> GetFilteredPriviledgesAsync(
        PriviledgeFilterDto filter,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Priviledges.GetQueryable();

        var pipeline = PriviledgeFilterPipeline.Create();
        var filteredQuery = pipeline.Execute(query, filter);

        var priviledges = await filteredQuery.ToListAsync(cancellationToken);
        return priviledges.Select(PriviledgeMapper.ToDto).ToList();
    }

    public async Task<ICollection<SelectListItemDto>> GetPriviledgeSelectListAsync(CancellationToken cancellationToken)
    {
        var priviledgesQuery = _unitOfWork.Priviledges.GetQueryable();

        var priviledgeList = await priviledgesQuery
            .Select(p => new SelectListItemDto
            {
                Id = p.Id.ToString(),
                Label = p.Name
            })
            .ToListAsync(cancellationToken) ?? [];

        return priviledgeList;
    }

    public async Task<PriviledgeDto> GetPriviledgeByIdAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        var priviledge = await _unitOfWork.Priviledges.GetAsync(
            p => p.Id == priviledgeId,
            cancellationToken
            ) ?? throw new PriviledgeNotFoundException(priviledgeId);

        return PriviledgeMapper.ToDto(priviledge);
    }

    public async Task<PriviledgeDto> AddPriviledgeAsync(PriviledgeDto priviledgeDto, CancellationToken cancellationToken)
    {
        await ValidatePriviledgeAsync(priviledgeDto, cancellationToken);

        var priviledge = PriviledgeMapper.ToEntity(priviledgeDto);

        _unitOfWork.Priviledges.Add(priviledge);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        priviledgeDto.Id = priviledge.Id;
        return priviledgeDto;
    }

    public async Task<PriviledgeDto> UpdatePriviledgeAsync(
        Guid priviledgeId,
        PriviledgeDto priviledgeDto,
        CancellationToken cancellationToken)
    {
        await ValidatePriviledgeAsync(priviledgeDto, cancellationToken);

        var priviledge = await _unitOfWork.Priviledges.GetAsync(
            p => p.Id == priviledgeId,
            cancellationToken
            ) ?? throw new PriviledgeNotFoundException(priviledgeId);

        priviledge.Name = priviledgeDto.Name!;
        priviledge.Description = priviledgeDto.Description;

        _unitOfWork.Priviledges.Update(priviledge);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        priviledgeDto.Id = priviledge.Id;
        return priviledgeDto;
    }

    public async Task DeletePriviledgeAsync(Guid priviledgeId, CancellationToken cancellationToken)
    {
        var priviledge = await _unitOfWork.Priviledges.GetAsync(
            p => p.Id == priviledgeId,
            cancellationToken
            ) ?? throw new PriviledgeNotFoundException(priviledgeId);

        _unitOfWork.Priviledges.Remove(priviledge);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidatePriviledgeAsync(PriviledgeDto priviledgeDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(priviledgeDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidPriviledgeRequestException(validationResult.ToString());
        }
    }
}
