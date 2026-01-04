using GSManager.Core.Models.DTOs;
using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Mappers;

public static class PlotMapper
{
    public static PlotDto ToDto(Plot plot)
    {
        return new PlotDto
        {
            Id = plot.Id,
            Number = plot.Number,
            Description = plot.Description,
            OwnerId = plot.OwnerId,
            Square = plot.Square,
            PriviledgeId = plot.PriviledgeId,
            CadastreNumber = plot.CadastreNumber
        };
    }

    public static Plot ToEntity(PlotDto plotDto, Member? owner = null, Priviledge? priviledge = null)
    {
        return new Plot
        {
            Id = plotDto.Id ?? Guid.NewGuid(),
            Number = plotDto.Number!,
            Description = plotDto.Description,
            OwnerId = owner?.Id,
            Owner = owner,
            Square = plotDto.Square,
            PriviledgeId = priviledge?.Id,
            Priviledge = priviledge,
            CadastreNumber = plotDto.CadastreNumber
        };
    }

    public static void UpdateEntity(Plot plot, PlotDto plotDto, Member? owner = null, Priviledge? priviledge = null)
    {
        plot.Number = plotDto.Number!;
        plot.Description = plotDto.Description;
        plot.OwnerId = owner?.Id;
        plot.Owner = owner;
        plot.Square = plotDto.Square;
        plot.PriviledgeId = priviledge?.Id;
        plot.Priviledge = priviledge;
        plot.CadastreNumber = plotDto.CadastreNumber;
    }
}
