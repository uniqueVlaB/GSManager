using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Role;

public static class RoleFilterPipeline
{
    public static FilterPipeline<Models.Entities.Society.Role, RoleFilterDto> Create()
    {
        return new FilterPipeline<Models.Entities.Society.Role, RoleFilterDto>()
            .AddFilter(new IdFilter())
            .AddFilter(new NameFilter())
            .AddFilter(new SearchQueryFilter())
            .AddFilter(new PriviledgeFilter());
    }
}
