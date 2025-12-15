using System.Linq.Expressions;

namespace GSManager.Core.Abstractions.Repository;

public interface IRepository<T>
    where T : class
{
    IQueryable<T> GetQueryable();
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken, string[]? includeProperties = null);
    Task<IList<T>> GetManyAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, string[]? includeProperties = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, string[]? includeProperties = null);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
