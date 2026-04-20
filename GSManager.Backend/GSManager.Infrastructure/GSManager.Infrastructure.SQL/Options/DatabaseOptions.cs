namespace GSManager.Infrastructure.SQL.Options;

public class DatabaseOptions
{
    public string Provider { get; set; } = "SqlServer";

    public string ConnectionString { get; set; } = string.Empty;
}