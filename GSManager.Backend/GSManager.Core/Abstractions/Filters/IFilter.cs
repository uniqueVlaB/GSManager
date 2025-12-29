namespace GSManager.Core.Abstractions.Filters;

public interface IFilter<T, TFilter>
{
    IQueryable<T> Apply(IQueryable<T> query, TFilter filter);
}
