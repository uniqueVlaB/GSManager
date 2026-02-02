using System.Diagnostics.Metrics;

namespace GSManager.API.Telemetry.EntityMetrics;

public class PlotMetrics
{
    public const string MeterName = "GSManager.API.Plots";

    private readonly Counter<long> _plotsCreated;
    private readonly Counter<long> _plotsDeleted;

    public PlotMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _plotsCreated = meter.CreateCounter<long>(
            name: "gsmanager.plots.created",
            unit: "{plot}",
            description: "Number of plots created");

        _plotsDeleted = meter.CreateCounter<long>(
            name: "gsmanager.plots.deleted",
            unit: "{plot}",
            description: "Number of plots deleted");
    }

    public void Created() => _plotsCreated.Add(1);

    public void Deleted() => _plotsDeleted.Add(1);
}
