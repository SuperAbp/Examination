namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// MySQL SQL语句提供者实现
/// </summary>
public class MySqlProvider : ISqlProvider
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