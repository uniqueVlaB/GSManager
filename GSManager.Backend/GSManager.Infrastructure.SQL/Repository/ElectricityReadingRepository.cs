using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Models.Entities.Electricity;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository;

public class ElectricityReadingRepository(ApplicationDbContext db) : Repository<ElectricityReading>(db), IElectricityReadingRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(ElectricityReading electricityReading)
    {
        _db.Update(electricityReading);
    }
}
