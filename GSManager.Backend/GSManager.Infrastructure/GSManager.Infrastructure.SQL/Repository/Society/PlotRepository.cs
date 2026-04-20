using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Core.Models.Entities.Society;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class PlotRepository(ApplicationDbContext db) : Repository<Plot>(db), IPlotRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Plot plot)
    {
        _db.Update(plot);
    }
}

