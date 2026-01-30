using GSManager.Core.Abstractions.Filters;
using GSManager.Core.Models.DTOs.Filters;

namespace GSManager.Core.Filters.Role;

public class NameFilter : IFilter<Models.Entities.Society.Role, RoleFilterDto>
{
    public IQueryable<Models.Entities.Society.Role> Apply(
        IQueryable<Models.Entities.Society.Role> query,
        RoleFilterDto filter)
    {
        if (string.IsNullOrWhiteSpace(filter.Name))
        {
            return query;
        }

        return query.Where(r => r.Name.Contains(filter.Name));
    }
}
