using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Society.Plot;

[ExcludeFromCodeCoverage]
public class InvalidPlotRequestException : GSManagerInvalidRequestException
{
    public InvalidPlotRequestException()
        : base("Invalid plot request.")
    {
    }

    public InvalidPlotRequestException(string message)
        : base(message)
    {
    }
}
