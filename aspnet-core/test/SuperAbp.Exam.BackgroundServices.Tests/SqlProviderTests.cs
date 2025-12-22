using SuperAbp.Exam.BackgroundServices.Sql;
using Xunit;

namespace SuperAbp.Exam.BackgroundServices.Tests;

/// <summary>
/// SQL提供者的单元测试示例
/// 演示了如何使用不同的SQL提供者
/// </summary>
public class SqlProviderTests
{
    [Fact]
    public void MySqlProvider_GetLastExecutedTime_ShouldUseLimitKeyword()
    {
        // Arrange
        var provider = new MySqlProvider();

        // Act
        var sql = provider.GetLastExecutedTime();

        // Assert
        Assert.Contains("LIMIT", sql);
        Assert.DoesNotContain("TOP", sql);
    }

    [Fact]
    public void SqlServerProvider_GetLastExecutedTime_ShouldUseTopsKeyword()
    {
        // Arrange
        var provider = new SqlServerProvider();

        // Act
        var sql = provider.GetLastExecutedTime();

        // Assert
        Assert.Contains("TOP", sql);
        Assert.DoesNotContain("LIMIT", sql);
    }

    [Fact]
    public void MySqlProvider_InsertPaper_ShouldUseUuid()
    {
        // Arrange
        var provider = new MySqlProvider();

        // Act
        var sql = provider.InsertPaper();

        // Assert
        Assert.Contains("UUID()", sql);
        Assert.Contains("REPLACE", sql);
        Assert.Contains("NOW()", sql);
    }

    [Fact]
    public void SqlServerProvider_InsertPaper_ShouldUseNewid()
    {
        // Arrange
        var provider = new SqlServerProvider();

        // Act
        var sql = provider.InsertPaper();

        // Assert
        Assert.Contains("NEWID()", sql);
        Assert.Contains("GETDATE()", sql);
    }

    [Fact]
    public void MySqlProvider_InsertQuestionAnswers_ShouldUseBacktickForReservedWords()
    {
        // Arrange
        var provider = new MySqlProvider();

        // Act
        var sql = provider.InsertQuestionAnswers();

        // Assert
        Assert.Contains("`Right`", sql);
    }

    [Fact]
    public void SqlServerProvider_InsertQuestionAnswers_ShouldUseBracketsForReservedWords()
    {
        // Arrange
        var provider = new SqlServerProvider();

        // Act
        var sql = provider.InsertQuestionAnswers();

        // Assert
        Assert.Contains("[Right]", sql);
    }

    [Fact]
    public void MySqlProvider_DeleteMethods_ShouldReturnValidSql()
    {
        // Arrange
        var provider = new MySqlProvider();

        // Act
        var deletePapersSql = provider.DeletePapers();
        var deleteQuestionsSql = provider.DeleteQuestions();
        var deleteExaminationsSql = provider.DeleteExaminations();

        // Assert
        Assert.Contains("DELETE FROM AppPapers", deletePapersSql);
        Assert.Contains("DELETE FROM AppQuestions", deleteQuestionsSql);
        Assert.Contains("DELETE FROM AppExaminations", deleteExaminationsSql);
        Assert.All(new[] { deletePapersSql, deleteQuestionsSql, deleteExaminationsSql },
            sql => Assert.Contains("WHERE TenantId = @TenantId", sql));
    }

    [Fact]
    public void SqlServerProvider_DeleteMethods_ShouldReturnValidSql()
    {
        // Arrange
        var provider = new SqlServerProvider();

        // Act
        var deletePapersSql = provider.DeletePapers();
        var deleteQuestionsSql = provider.DeleteQuestions();
        var deleteExaminationsSql = provider.DeleteExaminations();

        // Assert
        Assert.Contains("DELETE FROM AppPapers", deletePapersSql);
        Assert.Contains("DELETE FROM AppQuestions", deleteQuestionsSql);
        Assert.Contains("DELETE FROM AppExaminations", deleteExaminationsSql);
        Assert.All(new[] { deletePapersSql, deleteQuestionsSql, deleteExaminationsSql },
            sql => Assert.Contains("WHERE TenantId = @TenantId", sql));
    }

    [Fact]
    public void AllMethods_ShouldBeImplementedInBothProviders()
    {
        // Arrange
        var mySqlProvider = new MySqlProvider();
        var sqlServerProvider = new SqlServerProvider();
        var interfaceType = typeof(ISqlProvider);

        // Act
        var methods = interfaceType.GetMethods();

        // Assert - 确保所有接口方法都有实现且不返回null
        foreach (var method in methods)
        {
            var mySqlResult = method.Invoke(mySqlProvider, null);
            var sqlServerResult = method.Invoke(sqlServerProvider, null);

            Assert.NotNull(mySqlResult);
            Assert.NotNull(sqlServerResult);
            Assert.NotEmpty((string)mySqlResult!);
            Assert.NotEmpty((string)sqlServerResult!);
        }
    }
}
