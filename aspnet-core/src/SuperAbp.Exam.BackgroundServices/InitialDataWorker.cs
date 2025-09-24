using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace SuperAbp.Exam.BackgroundServices;

public class InitialDataWorker : AsyncPeriodicBackgroundWorkerBase
{
    public InitialDataWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 600_000;
        timer.RunOnStart = true;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        IConfiguration configuration = workerContext.ServiceProvider.GetRequiredService<IConfiguration>();
        ILogger<InitialDataWorker> logger = workerContext.ServiceProvider.GetRequiredService<ILogger<InitialDataWorker>>();

        using IDbConnection connection = new MySqlConnection(configuration.GetConnectionString("Default"));

        // TODO:Remove InitialDataExecutionLog Table
        DateTime lastExecutedTime = await connection.ExecuteScalarAsync<DateTime>("SELECT LastExecutedTime FROM InitialDataExecutionLog ORDER BY LastExecutedTime DESC LIMIT 1");
        if (!int.TryParse(configuration["InitialData:IntervalDays"], out int intervalDays))
        {
            intervalDays = 1;
        }
        if (!int.TryParse(configuration["InitialData:TargetHour"], out int targetHour))
        {
            targetHour = 1;
        }
        if ((Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00") - Convert.ToDateTime(lastExecutedTime.ToString("yyyy-MM-dd") + " 00:00:00")).Days < intervalDays)
        {
            logger.LogInformation("Initial data has already been executed today, skipping.");
            return;
        }
        if (DateTime.Now.Hour != targetHour)
        {
            return;
        }

        Guid? tenantId = await connection.QuerySingleOrDefaultAsync<Guid?>("SELECT Id FROM AbpTenants WHERE Name = @Name", new { Name = "Demo" });
        if (tenantId == null || tenantId == Guid.Empty)
        {
            logger.LogWarning("Demo tenant not found, skipping initial data.");
            return;
        }
        logger.LogDebug("Clear data……");
        await ClearDataAsync(connection, tenantId.Value);
        logger.LogDebug("Create Question……");
        bool flowControl = await CreateQuestionAsync(connection, tenantId.Value);
        if (!flowControl)
        {
            return;
        }
        await CreatePaperAsync(connection, tenantId.Value);
        logger.LogDebug("Created successfully.");

        await connection.ExecuteAsync("INSERT INTO InitialDataExecutionLog (LastExecutedTime) VALUES (@LastExecutedTime)", new { LastExecutedTime = DateTime.Now });
    }

    private static async Task CreatePaperAsync(IDbConnection connection, Guid tenantId)
    {
        Guid paperId = Guid.NewGuid();

        List<dynamic> exams = (await connection.QueryAsync<dynamic>("SELECT * FROM AppQuestionBanks WHERE TenantId = @TenantId",
            new { TenantId = tenantId })).ToList();

        int totalQuestionCount = 20;
        foreach (var exam in exams)
        {
            Guid paperIdLocal = Guid.NewGuid();
            Guid questionBankId = exam.Id;

            const int singleType = 0; // single choice
            const int multiType = 1; // multiple choice
            const int judgeType = 2; // judge
            const int blankType = 3; // blank

            var countsSql = @"SELECT QuestionType, COUNT(1) AS Cnt FROM AppQuestions WHERE QuestionBankId = @QuestionBankId AND TenantId = @TenantId GROUP BY QuestionType";
            var counts = (await connection.QueryAsync<dynamic>(countsSql, new { QuestionBankId = questionBankId, TenantId = tenantId })).ToDictionary(r => (int)r.QuestionType, r => (int)r.Cnt);

            int availableTotal = counts.Values.Sum();
            if (availableTotal == 0)
            {
                continue;
            }

            var types = counts.Keys.OrderBy(k => k).ToList();
            var target = types.ToDictionary(t => t, t => 0);
            int assigned = 0;

            while (assigned < totalQuestionCount)
            {
                bool allocatedInThisRound = false;
                foreach (var t in types)
                {
                    if (assigned >= totalQuestionCount) break;
                    if (target[t] < counts[t])
                    {
                        target[t]++;
                        assigned++;
                        allocatedInThisRound = true;
                    }
                }
                if (!allocatedInThisRound) break;
            }

            var selectedQuestionIds = new List<Guid>();
            foreach (var kv in target)
            {
                int need = kv.Value;
                if (need <= 0) continue;
                var sel = (await connection.QueryAsync<Guid>("SELECT Id FROM AppQuestions WHERE QuestionBankId = @QuestionBankId AND QuestionType = @QuestionType AND TenantId = @TenantId ORDER BY RAND() LIMIT @Limit", new { QuestionBankId = questionBankId, QuestionType = kv.Key, TenantId = tenantId, Limit = need })).ToList();
                selectedQuestionIds.AddRange(sel);
            }

            decimal totalScore = 0m;
            int singleCount = target.ContainsKey(0) ? target[0] : 0;
            int multiCount = target.ContainsKey(1) ? target[1] : 0;
            int judgeCount = target.ContainsKey(2) ? target[2] : 0;
            int blankCount = target.ContainsKey(3) ? target[3] : 0;

            totalScore = singleCount * 2m + multiCount * 4m + judgeCount * 2m + blankCount * 4m;

            var extraProps = JsonSerializer.Serialize(new { SelectedQuestionIds = selectedQuestionIds });
            await connection.ExecuteAsync(
                "INSERT INTO AppPapers (Id, Name, Description, TotalQuestionCount, Score, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp) VALUES (@Id, @Name, @Description, @TotalQuestionCount, @Score, @CreationTime, @TenantId, '{}', REPLACE(UUID(), '-', ''))",
                new { Id = paperIdLocal, Name = (exam.Title ?? "") + " 模拟试卷", Description = "", TotalQuestionCount = selectedQuestionIds.Count, Score = totalScore, CreationTime = DateTime.Now, TenantId = tenantId });

            var passingScore = Math.Floor(Math.Round((double)totalScore * 0.6));
            await connection.ExecuteAsync(
                @"INSERT INTO AppExamination (Id, Name, Description, Score, PassingScore, TotalTime, PaperId, Status, AnswerMode, RandomOrderOfOption, StartTime, EndTime, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
                    VALUES (@Id, @Name, @Description, @Score, @PassingScore, @TotalTime, @PaperId, @Status, @AnswerMode, @RandomOrderOfOption, @StartTime, @EndTime, @CreationTime, @TenantId, '{}', REPLACE(UUID(), '-', ''))",
                new
                {
                    Id = Guid.NewGuid(),
                    Name = (exam.Title ?? "") + " 考试",
                    Description = "",
                    Score = totalScore,
                    PassingScore = (decimal)passingScore,
                    TotalTime = 60,
                    PaperId = paperIdLocal,
                    Status = 1, // Draft
                    AnswerMode = 0, // All
                    RandomOrderOfOption = false,
                    StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"),
                    EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00").AddDays(10),
                    CreationTime = DateTime.Now,
                    TenantId = tenantId
                });

            await connection.ExecuteAsync(
                "INSERT INTO AppPaperQuestionRules (Id, PaperId, QuestionBankId, Proportion, SingleCount, SingleScore, MultiCount, MultiScore, JudgeCount, JudgeScore, BlankCount, BlankScore, CreationTime, TenantId) VALUES (@Id, @PaperId, @QuestionBankId, @Proportion, @SingleCount, @SingleScore, @MultiCount, @MultiScore, @JudgeCount, @JudgeScore, @BlankCount, @BlankScore, @CreationTime, @TenantId)",
                new
                {
                    Id = Guid.NewGuid(),
                    PaperId = paperIdLocal,
                    QuestionBankId = questionBankId,
                    Proportion = 1.0m,
                    SingleCount = target.ContainsKey(singleType) ? (int?)target[singleType] : 0,
                    SingleScore = (target.ContainsKey(singleType) && target[singleType] > 0) ? 2m : 0m,
                    MultiCount = target.ContainsKey(multiType) ? (int?)target[multiType] : 0,
                    MultiScore = (target.ContainsKey(multiType) && target[multiType] > 0) ? 4m : 0m,
                    JudgeCount = target.ContainsKey(judgeType) ? (int?)target[judgeType] : 0,
                    JudgeScore = (target.ContainsKey(judgeType) && target[judgeType] > 0) ? 2m : 0m,
                    BlankCount = target.ContainsKey(blankType) ? (int?)target[blankType] : 0,
                    BlankScore = (target.ContainsKey(blankType) && target[blankType] > 0) ? 4m : 0m,
                    CreationTime = DateTime.Now,
                    TenantId = tenantId
                });
        }
    }

    private static async Task<bool> CreateQuestionAsync(IDbConnection connection, Guid tenantId)
    {
        var jsonPath = Path.Combine(AppContext.BaseDirectory, "questions.json");
        if (!File.Exists(jsonPath))
        {
            return false;
        }
        string json = await File.ReadAllTextAsync(jsonPath);
        var doc = JsonSerializer.Deserialize<QuestionsFile>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        if (doc?.QuesiontBanks == null || doc.QuesiontBanks.Count == 0)
        {
            return false;
        }

        var bankParams = new List<object>();
        var questionParams = new List<object>();
        var answerParams = new List<object>();
        var knowledgepointParams = new List<object>(); // question-knowledgepoint relations

        var seenKnowledgePoints = new HashSet<string>();

        // insert banks, questions and options (collect params first)
        foreach (var bank in doc.QuesiontBanks)
        {
            var bankId = Guid.NewGuid();
            bankParams.Add(new { Id = bankId, Title = bank.Name, Remark = bank.Description, CreationTime = DateTime.Now, TenantId = tenantId });

            if (bank.Questions == null) continue;

            foreach (var q in bank.Questions)
            {
                var qid = Guid.NewGuid();
                // note: DB column for question text is `Content` (per migrations), map DTO Title -> Content
                questionParams.Add(new { Id = qid, QuestionBankId = bankId, QuestionType = q.Type, Content = q.Title ?? string.Empty, CreationTime = DateTime.Now, TenantId = tenantId, Analysis = q.Analysis });

                if (q.KnowledgePoints != null)
                {
                    var newKps = q.KnowledgePoints.Except(seenKnowledgePoints).ToList();
                    foreach (var kpId in newKps)
                    {
                        seenKnowledgePoints.Add(kpId);
                    }

                    // link all KP for this question
                    foreach (var kpId in q.KnowledgePoints)
                    {
                        knowledgepointParams.Add(new { QuestionId = qid, KnowledgePointId = kpId, CreationTime = DateTime.Now, TenantId = tenantId });
                    }
                }

                if (q.Options == null) continue;

                foreach (var opt in q.Options.Select((o, idx) => new { Opt = o, Index = idx }))
                {
                    answerParams.Add(new { Id = Guid.NewGuid(), QuestionId = qid, Content = opt.Opt.Content, Right = opt.Opt.Right, Ordinal = opt.Index + 1, CreationTime = DateTime.Now, TenantId = tenantId });
                }
            }
        }

        if (bankParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionBanks (Id, Title, Remark, ExtraProperties, ConcurrencyStamp, CreationTime, TenantId) VALUES (@Id, @Title, @Remark, '{}', REPLACE(UUID(), '-', ''), @CreationTime, @TenantId)", bankParams);
        }
        if (questionParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestions (Id, QuestionBankId, QuestionType, Content, Analysis, ExtraProperties, ConcurrencyStamp, CreationTime, TenantId) VALUES (@Id, @QuestionBankId, @QuestionType, @Content, @Analysis, '{}', REPLACE(UUID(), '-', ''), @CreationTime, @TenantId)", questionParams);
        }
        if (answerParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionAnswers (Id, QuestionId, Content, `Right`, Sort, CreationTime, TenantId) VALUES (@Id, @QuestionId, @Content, @Right, @Ordinal, @CreationTime, @TenantId)", answerParams);
        }
        if (knowledgepointParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionKnowledgePoints (QuestionId, KnowledgePointId, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp) VALUES (@QuestionId, @KnowledgePointId, @CreationTime, @TenantId, '{}', REPLACE(UUID(), '-', ''))", knowledgepointParams);
        }

        return true;
    }

    private async Task ClearDataAsync(IDbConnection connection, Guid tenantId)
    {
        await connection.ExecuteAsync("DELETE FROM AppQuestionBanks WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionAnswers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestions WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionKnowledgePoints WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPapers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPaperQuestionRules WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppExamination WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppKnowledgePoints WHERE TenantId = @TenantId", new { TenantId = tenantId });
    }

    // DTOs for parsing questions.json
    public class QuestionsFile
    {
        public List<QuestionBankDto>? QuesiontBanks { get; set; }
    }

    public class QuestionBankDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<QuestionDto>? Questions { get; set; }
    }

    public class QuestionDto
    {
        public string? Title { get; set; }
        public int Type { get; set; }
        public string? Analysis { get; set; }
        public string[] KnowledgePoints { get; set; } = [];
        public List<OptionDto>? Options { get; set; }
    }

    public class OptionDto
    {
        public string? Content { get; set; }
        public bool Right { get; set; }
    }
}