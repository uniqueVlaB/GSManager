using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Plot;

[ExcludeFromCodeCoverage]
public class PlotNotFoundException : GSManagerNotFoundException
{
    public PlotNotFoundException(Guid id)
        : base($"Plot with id '{id}' not found.")
    {
    }

    public PlotNotFoundException()
        : base("Plot not found.")
    {
    }
}
