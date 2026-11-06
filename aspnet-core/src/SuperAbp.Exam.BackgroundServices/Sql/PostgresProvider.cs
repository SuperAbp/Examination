namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// PostgreSQL SQL语句提供者实现
/// </summary>
public class PostgresProvider : ISqlProvider
{
    public string GetLastExecutedTime()
    {
        return "SELECT LastExecutedTime FROM InitialDataExecutionLog ORDER BY LastExecutedTime DESC LIMIT 1";
    }

    public string InsertInitialDataExecutionLog()
    {
        return "INSERT INTO InitialDataExecutionLog (LastExecutedTime) VALUES (@LastExecutedTime)";
    }
}
