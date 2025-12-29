using GSManager.Core.Models.DTOs;

namespace GSManager.Core.Filters.Member;

public static class MemberFilterPipeline
{
    public static FilterPipeline<Models.Entities.Society.Member, MemberFilterDto> Create()
    {
        return new FilterPipeline<Models.Entities.Society.Member, MemberFilterDto>()
            .AddFilter(new FirstNameFilter())
            .AddFilter(new LastNameFilter())
            .AddFilter(new EmailFilter())
            .AddFilter(new RoleFilter())
            .AddFilter(new PriviledgeFilter())
            .AddFilter(new PlotFilter());
    }
}
