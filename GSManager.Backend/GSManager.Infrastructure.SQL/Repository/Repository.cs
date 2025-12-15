using System.Linq.Expressions;
using GSManager.Core.Abstractions.Repository;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Infrastructure.SQL.Repository;

public class Repository<T> : IRepository<T>
    where T : class
{
    protected readonly DbSet<T> DbSet;

    public Repository(ApplicationDbContext db)
    {
        var dbContext = db;
        DbSet = dbContext.Set<T>();
    }

    public void Add(T entity)
    {
        DbSet.Add(entity);
    }

    public IQueryable<T> GetQueryable()
    {
        return DbSet;
    }

    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, string[]? includeProperties = null)
    {
        IQueryable<T> query = DbSet;
        query = ApplyIncludes(query, includeProperties);

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, string[]? includeProperties = null)
    {
        IQueryable<T> query = DbSet;
        query = ApplyIncludes(query, includeProperties);

        query = query.Where(filter);

        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, string[]? includeProperties = null)
    {
        IQueryable<T> query = DbSet;
        query = ApplyIncludes(query, includeProperties);

        query = query.Where(filter);
        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    protected static IQueryable<T> ApplyIncludes(IQueryable<T> query, string[]? includeProperties)
    {
        if (includeProperties == null)
        {
            return query;
        }

        foreach (var include in includeProperties.Where(include => !string.IsNullOrEmpty(include)))
        {
            query = query.Include(include);
        }

        return query;
    }
}
