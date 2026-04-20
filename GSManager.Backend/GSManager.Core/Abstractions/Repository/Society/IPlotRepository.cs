using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Abstractions.Repository.Society;

public interface IPlotRepository : IRepository<Plot>
{
    void Update(Plot plot);
}

