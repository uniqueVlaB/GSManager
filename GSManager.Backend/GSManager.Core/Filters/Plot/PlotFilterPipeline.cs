using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Plot;

public static class PlotFilterPipeline
{
    public static FilterPipeline<Models.Entities.Society.Plot, PlotFilterDto> Create()
    {
        return new FilterPipeline<Models.Entities.Society.Plot, PlotFilterDto>()
            .AddFilter(new IdFilter())
            .AddFilter(new NumberFilter())
            .AddFilter(new OwnerFilter())
            .AddFilter(new CadastreNumberFilter())
            .AddFilter(new PlotPriviledgeFilter());
    }
}
