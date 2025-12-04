using Microsoft.Extensions.Configuration;
using System;

namespace SuperAbp.Exam.BackgroundServices.Sql;

/// <summary>
/// SQL提供者工厂
/// 根据配置选择合适的数据库实现
/// </summary>
public class SqlProviderFactory
{
    /// <summary>
    /// 根据配置创建SQL提供者
    /// </summary>
    /// <param name="configuration">应用程序配置</param>
    /// <returns>SQL提供者实例</returns>
    public static ISqlProvider CreateProvider(string databaseType)
    {
        return databaseType?.ToLower() switch
        {
            "mysql" => new MySqlProvider(),
            "sqlserver" => new SqlServerProvider(),
            _ => throw new InvalidOperationException($"不支持的数据库类型: {databaseType}")
        };
    }

    /// <summary>
    /// 获取默认的SQL提供者（MySQL）
    /// </summary>
    /// <returns>MySQL提供者实例</returns>
    public static ISqlProvider GetDefaultProvider()
    {
        return new MySqlProvider();
    }
}