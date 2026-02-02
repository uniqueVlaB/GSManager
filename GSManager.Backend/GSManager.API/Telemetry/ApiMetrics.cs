using System.Diagnostics.Metrics;

namespace GSManager.API.Telemetry;

public class ApiMetrics
{
    public const string MeterName = "GSManager.Api";

    private readonly Counter<long> _criticalErrors;
    private readonly Counter<long> _wrongRequests;
    private readonly Counter<long> _dbExceptions;

    public ApiMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _criticalErrors = meter.CreateCounter<long>(
            name: "gsmanager.critical.errors",
            unit: "{error}",
            description: "Number of critical errors");

        _wrongRequests = meter.CreateCounter<long>(
            name: "gsmanager.wrong.requests",
            unit: "{request}",
            description: "Number of wrong requests (client's input is invalid or requested resource don't exist)");
        
        _dbExceptions = meter.CreateCounter<long>(
            name: "gsmanager.db.exceptions",
            unit: "{exception}",
            description: "Number of database exceptions");

    }

    public void CriticalError()
    {
        _criticalErrors.Add(1);
    }

    public void WrongRequest()
    {
        _wrongRequests.Add(1);
    }

    public void DbException()
    {
        _dbExceptions.Add(1);
    }
}
