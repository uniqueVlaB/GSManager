using System.Diagnostics.Metrics;
using GSManager.API.Telemetry.EntityMetrics;

namespace GSManager.API.Telemetry;

public class ApiMeters(IMeterFactory meterFactory)
{
    public ApiMetrics Api { get; private set; } = new ApiMetrics(meterFactory);

    //Entity Metrics
    public MemberMetrics Member { get; private set; } = new MemberMetrics(meterFactory);
    public PlotMetrics Plot { get; private set; } = new PlotMetrics(meterFactory);
    public RoleMetrics Role { get; private set; } = new RoleMetrics(meterFactory);
}
