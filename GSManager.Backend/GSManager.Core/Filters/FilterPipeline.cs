using GSManager.Core.Abstractions.Filters;

namespace GSManager.Core.Filters;

public class FilterPipeline<T, TFilter>
{
    private readonly List<IFilter<T, TFilter>> _filters = [];

    public FilterPipeline<T, TFilter> AddFilter(IFilter<T, TFilter> filter)
    {
        _filters.Add(filter);
        return this;
    }

    public IQueryable<T> Execute(IQueryable<T> query, TFilter filter)
    {
        return _filters.Aggregate(query, (current, f) => f.Apply(current, filter));
    }
}
