using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository;

public interface IPlotRepository : IRepository<Plot>
{
    void Update(Plot plot);
}
