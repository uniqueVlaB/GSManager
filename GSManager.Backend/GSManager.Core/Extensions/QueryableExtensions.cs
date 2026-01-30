using System.Linq.Expressions;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResultDto<TDto>> ToPagedResultDtoAsync<TEntity, TDto, TKey>(
        this IQueryable<TEntity> query,
        int pageNumber,
        int pageSize,
        Func<TEntity, TDto> mapper,
        Expression<Func<TEntity, TKey>> orderByKeySelector,
        CancellationToken cancellationToken,
        bool descending = false,
        int maxPageSize = 100)
    {

        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(orderByKeySelector);

        pageNumber = Math.Max(pageNumber, 1);
        pageSize = pageSize < 1 ? maxPageSize : Math.Clamp(pageSize, 1, maxPageSize);

        var totalCount = await query.CountAsync(cancellationToken);

        var orderedQuery = descending
            ? query.OrderByDescending(orderByKeySelector)
            : query.OrderBy(orderByKeySelector);

        var items = await orderedQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<TDto>
        {
            Items = [.. items.Select(mapper)],
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };
    }
}
