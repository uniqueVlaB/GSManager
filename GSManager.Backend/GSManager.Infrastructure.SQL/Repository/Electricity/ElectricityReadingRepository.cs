using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Electricity;
using GSManager.Core.Models.Entities.Electricity;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Electricity;

public class ElectricityReadingRepository(ApplicationDbContext db) : Repository<ElectricityReading>(db), IElectricityReadingRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(ElectricityReading electricityReading)
    {
        _db.Update(electricityReading);
    }
}

