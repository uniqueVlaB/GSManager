using GSManager.Core.Models.Entities.Electricity;

namespace GSManager.Core.Abstractions.Repository;

public interface IElectricityMeterRepository : IRepository<ElectricityMeter>
{
    void Update(ElectricityMeter meter);
}
