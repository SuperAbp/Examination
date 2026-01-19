namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// SQL语句提供者接口
/// 用于不同数据库实现提供各自的SQL语句
/// </summary>
public interface ISqlProvider
{
    /// <summary>
    /// 获取最后执行时间
    /// </summary>
    string GetLastExecutedTime();

    /// <summary>
    /// 插入初始数据执行日志
    /// </summary>
    string InsertInitialDataExecutionLog();
}