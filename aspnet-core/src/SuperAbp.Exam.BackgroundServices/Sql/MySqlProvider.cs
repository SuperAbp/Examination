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

    public string GetTenantIdByName()
    {
        return "SELECT Id FROM AbpTenants WHERE Name = @Name";
    }

    public string GetQuestionBanks()
    {
        return "SELECT * FROM AppQuestionBanks WHERE TenantId = @TenantId";
    }

    public string GetQuestionCountsByType()
    {
        return @"SELECT QuestionType, COUNT(1) AS Cnt
                 FROM AppQuestions
                 WHERE QuestionBankId = @QuestionBankId AND TenantId = @TenantId
                 GROUP BY QuestionType
                 HAVING COUNT(1) > 0";
    }

    public string GetQuestions()
    {
        return @"SELECT Id, QuestionType FROM AppQuestions
                 WHERE QuestionBankId = @QuestionBankId
                 AND TenantId = @TenantId
                 ORDER BY CreationTime DESC";
    }

    public string GetPapers()
    {
        return "SELECT * FROM AppPapers WHERE TenantId = @TenantId";
    }

    public string InsertPaper()
    {
        return @"INSERT INTO AppPapers (Id, Name, PaperType, Description, TotalQuestionCount, Score, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
                 VALUES (@Id, @Name, @PaperType, @Description, @TotalQuestionCount, @Score, NOW(), @TenantId, '{}', REPLACE(UUID(), '-', ''))";
    }

    public string InsertPaperSections()
    {
        return @"INSERT INTO AppPaperSections (Id, PaperId, Title, ScoreEach, `Order`, TotalScore, TotalCount, CreationTime, TenantId)
                 VALUES (@Id, @PaperId, @Title, @ScoreEach, @Order, @TotalScore, @TotalCount, NOW(), @TenantId)";
    }

    public string InsertPaperQuestionRules()
    {
        return @"INSERT INTO AppPaperQuestionRules (Id, PaperSectionId, QuestionBankId, QuestionType, Count, Score, CreationTime, TenantId)
                 VALUES (@Id, @PaperSectionId, @QuestionBankId, @QuestionType, @Count, @Score, NOW(), @TenantId)";
    }

    public string InsertPaperQuestions()
    {
        return @"INSERT INTO AppPaperQuestions (Id, PaperSectionId, QuestionId, Score, `Order`, CreationTime, TenantId)
                 VALUES (@Id, @PaperSectionId, @QuestionId, @Score, @Order, NOW(), @TenantId)";
    }

    public string InsertExaminations()
    {
        return @"INSERT INTO AppExaminations (Id, Name, Description, Score, PassingScore, TotalTime, PaperId, Status,
                 AnswerMode, RandomOrderOfOption, StartTime, EndTime, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
                 VALUES (@Id, @Name, @Description, @Score, @PassingScore, @TotalTime, @PaperId, @Status, @AnswerMode,
                 @RandomOrderOfOption, @StartTime, @EndTime, NOW(), @TenantId, '{}', REPLACE(UUID(), '-', ''))";
    }

    public string InsertInitialDataExecutionLog()
    {
        return "INSERT INTO InitialDataExecutionLog (LastExecutedTime) VALUES (@LastExecutedTime)";
    }

    public string DeletePaperQuestions()
    {
        return "DELETE FROM AppPaperQuestions WHERE TenantId = @TenantId";
    }

    public string DeletePaperQuestionRules()
    {
        return "DELETE FROM AppPaperQuestionRules WHERE TenantId = @TenantId";
    }

    public string DeletePaperSections()
    {
        return "DELETE FROM AppPaperSections WHERE TenantId = @TenantId";
    }

    public string DeletePapers()
    {
        return "DELETE FROM AppPapers WHERE TenantId = @TenantId";
    }

    public string DeleteUserExamQuestions()
    {
        return "DELETE FROM AppUserExamQuestions WHERE TenantId = @TenantId";
    }

    public string DeleteUserExamSections()
    {
        return "DELETE FROM AppUserExamSections WHERE TenantId = @TenantId";
    }

    public string DeleteUserExams()
    {
        return "DELETE FROM AppUserExams WHERE TenantId = @TenantId";
    }

    public string DeleteKnowledgePoints()
    {
        return "DELETE FROM AppKnowledgePoints WHERE TenantId = @TenantId";
    }

    public string DeleteExaminations()
    {
        return "DELETE FROM AppExaminations WHERE TenantId = @TenantId";
    }

    public string DeleteQuestionOptions()
    {
        return "DELETE FROM AppQuestionOptions WHERE TenantId = @TenantId";
    }

    public string DeleteQuestions()
    {
        return "DELETE FROM AppQuestions WHERE TenantId = @TenantId";
    }

    public string DeleteQuestionKnowledgePoints()
    {
        return "DELETE FROM AppQuestionKnowledgePoints WHERE TenantId = @TenantId";
    }

    public string DeleteQuestionBanks()
    {
        return "DELETE FROM AppQuestionBanks WHERE TenantId = @TenantId";
    }

    public string InsertQuestionBanks()
    {
        return "INSERT INTO AppQuestionBanks (Id, Title, Remark, ExtraProperties, ConcurrencyStamp, CreationTime, TenantId) VALUES (@Id, @Title, @Remark, '{}', REPLACE(UUID(), '-', ''), @CreationTime, @TenantId)";
    }

    public string InsertQuestions()
    {
        return "INSERT INTO AppQuestions (Id, QuestionBankId, QuestionType, Content, Analysis, ExtraProperties, ConcurrencyStamp, CreationTime, TenantId) VALUES (@Id, @QuestionBankId, @QuestionType, @Content, @Analysis, '{}', REPLACE(UUID(), '-', ''), @CreationTime, @TenantId)";
    }

    public string InsertQuestionOptions()
    {
        return "INSERT INTO AppQuestionAnswers (Id, QuestionId, Content, `Right`, Sort, CreationTime, TenantId) VALUES (@Id, @QuestionId, @Content, @Right, @Sort, @CreationTime, @TenantId)";
    }

    public string InsertQuestionKnowledgePoints()
    {
        return "INSERT INTO AppQuestionKnowledgePoints (QuestionId, KnowledgePointId, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp) VALUES (@QuestionId, @KnowledgePointId, @CreationTime, @TenantId, '{}', REPLACE(UUID(), '-', ''))";
    }
}