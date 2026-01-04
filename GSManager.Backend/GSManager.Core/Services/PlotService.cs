using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Exceptions.Member;
using GSManager.Core.Exceptions.Plot;
using GSManager.Core.Exceptions.Priviledge;
using GSManager.Core.Filters.Plot;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs;
using GSManager.Core.Models.Entities.Society;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services;

public class PlotService(
    IUnitOfWork unitOfWork,
    IValidator<PlotDto> validator
    ) : IPlotService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PlotDto> _validator = validator;

    public async Task<ICollection<PlotDto>> GetAllPlotsAsync(CancellationToken cancellationToken)
    {
        var plots = await _unitOfWork.Plots.GetAllAsync(cancellationToken);
        
        return [.. plots.Select(PlotMapper.ToDto)];
    }

    public async Task<ICollection<PlotDto>> GetFilteredPlotsAsync(
        PlotFilterDto filter,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Plots.GetQueryable();

        var pipeline = PlotFilterPipeline.Create();
        var filteredQuery = pipeline.Execute(query, filter);

        var plots = await filteredQuery.ToListAsync(cancellationToken);
        return plots.Select(PlotMapper.ToDto).ToList();
    }

    public async Task<PlotDto> GetPlotByIdAsync(Guid plotId, CancellationToken cancellationToken)
    {
        var plot = await _unitOfWork.Plots.GetAsync(
            p => p.Id == plotId,
            cancellationToken,
            includeProperties: [nameof(Plot.Owner), nameof(Plot.Priviledge)]
            ) ?? throw new PlotNotFoundException(plotId);

        return PlotMapper.ToDto(plot);
    }

    public async Task<PlotDto> AddPlotAsync(PlotDto plotDto, CancellationToken cancellationToken)
    {
        await ValidatePlotAsync(plotDto, cancellationToken);

        var owner = await ResolveOwnerAsync(plotDto.OwnerId, cancellationToken);
        var priviledge = await ResolvePriviledgeAsync(plotDto.PriviledgeId, cancellationToken);

        plotDto.Id = null;
        var plot = PlotMapper.ToEntity(plotDto, owner, priviledge);

        _unitOfWork.Plots.Add(plot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        plotDto.Id = plot.Id;
        return plotDto;
    }

    public async Task<PlotDto> UpdatePlotAsync(Guid plotId, PlotDto plotDto, CancellationToken cancellationToken)
    {
        await ValidatePlotAsync(plotDto, cancellationToken);

        var plot = await _unitOfWork.Plots.GetAsync(
            p => p.Id == plotId,
            cancellationToken
            ) ?? throw new PlotNotFoundException(plotId);

        var owner = await ResolveOwnerAsync(plotDto.OwnerId, cancellationToken);
        var priviledge = await ResolvePriviledgeAsync(plotDto.PriviledgeId, cancellationToken);

        PlotMapper.UpdateEntity(plot, plotDto, owner, priviledge);
        _unitOfWork.Plots.Update(plot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        plotDto.Id = plot.Id;
        return plotDto;
    }

    public async Task DeletePlotAsync(Guid plotId, CancellationToken cancellationToken)
    {
        var plot = await _unitOfWork.Plots.GetAsync(
            p => p.Id == plotId,
            cancellationToken
            ) ?? throw new PlotNotFoundException(plotId);

        _unitOfWork.Plots.Remove(plot);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidatePlotAsync(PlotDto plotDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(plotDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidPlotRequestException(validationResult.ToString());
        }
    }

    private async Task<Member?> ResolveOwnerAsync(Guid? ownerId, CancellationToken cancellationToken)
    {
        if (ownerId is null)
        {
            return null;
        }

        var id = ownerId.Value;
        var member = await _unitOfWork.Members.GetAsync(m => m.Id == id, cancellationToken);
        return member ?? throw new MemberNotFoundException(id);
    }

    private async Task<Priviledge?> ResolvePriviledgeAsync(Guid? priviledgeId, CancellationToken cancellationToken)
    {
        if (priviledgeId is null)
        {
            return null;
        }

        var id = priviledgeId.Value;
        var priviledge = await _unitOfWork.Priviledges.GetAsync(p => p.Id == id, cancellationToken);
        return priviledge ?? throw new PriviledgeNotFoundException(id);
    }
}
