using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Electricity;
using GSManager.Core.Models.Entities.Electricity;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Electricity;

public class ElectricityMeterRepository(ApplicationDbContext db) : Repository<ElectricityMeter>(db), IElectricityMeterRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(ElectricityMeter electricityMeter)
    {
        _db.Update(electricityMeter);
    }
}

