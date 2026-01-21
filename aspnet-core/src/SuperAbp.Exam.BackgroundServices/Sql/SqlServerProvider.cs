namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// SQL Server SQL语句提供者实现
/// </summary>
public class SqlServerProvider : ISqlProvider
{
    public string GetLastExecutedTime()
    {
        return "SELECT TOP 1 LastExecutedTime FROM InitialDataExecutionLog ORDER BY LastExecutedTime DESC";
    }

    public string InsertInitialDataExecutionLog()
    {
        return "INSERT INTO InitialDataExecutionLog (LastExecutedTime) VALUES (@LastExecutedTime)";
    }
}