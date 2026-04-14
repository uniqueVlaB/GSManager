using GSManager.Core.Models.Entities.Electricity;

namespace GSManager.Core.Abstractions.Repository;

public interface IElectricityReadingRepository : IRepository<ElectricityReading>
{
    void Update(ElectricityReading reading);
}
