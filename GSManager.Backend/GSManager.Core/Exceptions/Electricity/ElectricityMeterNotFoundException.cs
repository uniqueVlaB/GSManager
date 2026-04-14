using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Electricity;

[ExcludeFromCodeCoverage]
public class ElectricityMeterNotFoundException : GSManagerNotFoundException
{
    public ElectricityMeterNotFoundException(Guid id)
        : base($"Electricity meter with id '{id}' not found.")
    {
    }

    public ElectricityMeterNotFoundException()
        : base("Electricity meter not found.")
    {
    }
}
