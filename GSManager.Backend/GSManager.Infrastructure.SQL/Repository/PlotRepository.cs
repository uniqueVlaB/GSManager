using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository;

public class PlotRepository(ApplicationDbContext db) : Repository<Plot>(db), IPlotRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Plot plot)
    {
        _db.Update(plot);
    }
}
