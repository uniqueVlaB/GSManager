using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Electricity;

[ExcludeFromCodeCoverage]
public class InvalidElectricityMeterRequestException : GSManagerInvalidRequestException
{
    public InvalidElectricityMeterRequestException()
        : base("Invalid electricity meter request.")
    {
    }

    public InvalidElectricityMeterRequestException(string message)
        : base(message)
    {
    }
}
