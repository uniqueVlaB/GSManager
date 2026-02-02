using System.Diagnostics.Metrics;

namespace GSManager.API.Telemetry.EntityMetrics;

public class RoleMetrics
{
    public const string MeterName = "GSManager.API.Roles";

    private readonly Counter<long> _rolesCreated;
    private readonly Counter<long> _rolesDeleted;

    public RoleMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _rolesCreated = meter.CreateCounter<long>(
            name: "gsmanager.roles.created",
            unit: "{role}",
            description: "Number of roles created");

        _rolesDeleted = meter.CreateCounter<long>(
            name: "gsmanager.roles.deleted",
            unit: "{role}",
            description: "Number of roles deleted");
    }

    public void Created() => _rolesCreated.Add(1);

    public void Deleted() => _rolesDeleted.Add(1);
}
