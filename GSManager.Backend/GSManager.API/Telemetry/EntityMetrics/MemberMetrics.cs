using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace GSManager.API.Telemetry.EntityMetrics;

public class MemberMetrics
{
    public const string MeterName = "GSManager.API.Members";

    private readonly Counter<long> _membersCreated;
    private readonly Counter<long> _membersDeleted;
    private readonly Counter<long> _membersUpdated;

    public MemberMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _membersCreated = meter.CreateCounter<long>(
            name: "gsmanager.members.created",
            unit: "{member}",
            description: "Number of members created");

        _membersDeleted = meter.CreateCounter<long>(
            name: "gsmanager.members.deleted",
            unit: "{member}",
            description: "Number of members deleted");

        _membersUpdated = meter.CreateCounter<long>(
            name: "gsmanager.members.updated",
            unit: "{member}",
            description: "Number of members updated");
    }

    public void Created(string? roleId = null)
    {
        var tags = new TagList
        {
            { "role_id", roleId ?? "none" }
        };

        _membersCreated.Add(1, tags);
    }

    public void Deleted()
    {
        _membersDeleted.Add(1);
    }

    public void Updated()
    {
        _membersUpdated.Add(1);
    }
}
