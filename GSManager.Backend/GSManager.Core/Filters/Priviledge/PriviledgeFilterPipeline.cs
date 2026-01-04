using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Priviledge;

public static class PriviledgeFilterPipeline
{
    public static FilterPipeline<Models.Entities.Society.Priviledge, PriviledgeFilterDto> Create()
    {
        return new FilterPipeline<Models.Entities.Society.Priviledge, PriviledgeFilterDto>()
            .AddFilter(new IdFilter())
            .AddFilter(new NameFilter());
    }
}
