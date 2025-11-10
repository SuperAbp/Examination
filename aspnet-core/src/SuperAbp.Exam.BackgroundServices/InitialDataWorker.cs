using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using static System.Collections.Specialized.BitVector32;

namespace SuperAbp.Exam.BackgroundServices;

public class InitialDataWorker : AsyncPeriodicBackgroundWorkerBase
{
    public InitialDataWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 600_000;
        timer.RunOnStart = true;
    }

    private const int CountForSection = 10;
    private static readonly Random _rand = new Random();

    private Dictionary<int, decimal> QuestionTypeScores = new()
    {
        {0, 2 },{1, 4 },{2, 2 },{3, 5 }
    };

    private Dictionary<int, string> QuestionTypeNames = new()
    {
        {0, "单选题" },{1, "多选题" },{2, "判断题" },{3, "填空题" }
    };

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
#if DEBUG
#else
        if (DateTime.Now.Hour != targetHour)
        {
            return;
        }
#endif

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
        logger.LogDebug("Create Paper……");
        await CreatePaperAsync(connection, tenantId.Value);
        logger.LogDebug("Create Exam……");
        await CreateExamAsync(connection, tenantId.Value);
        logger.LogDebug("Create Record……");
        await connection.ExecuteAsync("INSERT INTO InitialDataExecutionLog (LastExecutedTime) VALUES (@LastExecutedTime)", new { LastExecutedTime = DateTime.Now });
        logger.LogDebug("Created successfully.");
    }

    private async Task CreatePaperAsync(IDbConnection connection, Guid tenantId)
    {
        List<dynamic> questionBanks = (await connection.QueryAsync<dynamic>(
            "SELECT * FROM AppQuestionBanks WHERE TenantId = @TenantId",
            new { TenantId = tenantId })).ToList();

        foreach (var questionBank in questionBanks)
        {
            Guid questionBankId = questionBank.Id;
            string bankName = questionBank.Title ?? "";

            await CreateRandomPaper(connection, tenantId, questionBankId, bankName);
            await CreateFixedPaper(connection, tenantId, questionBankId, bankName);
        }
    }

    private async Task CreateRandomPaper(IDbConnection connection, Guid tenantId, Guid questionBankId, string bankName)
    {
        Guid paperId = Guid.NewGuid();
        string countsSql = @"SELECT QuestionType, COUNT(1) AS Cnt
                             FROM AppQuestions
                             WHERE QuestionBankId = @QuestionBankId AND TenantId = @TenantId
                             GROUP BY QuestionType
                             HAVING COUNT(1) > 0";
        Dictionary<int, int> counts = (await connection.QueryAsync<dynamic>(countsSql, new { QuestionBankId = questionBankId, TenantId = tenantId }))
                .ToDictionary(r => (int)r.QuestionType, r => (int)r.Cnt);

        List<PaperSection> sections = [];
        List<PaperQuestionRule> rules = [];
        int sectionOrder = 1;
        foreach (KeyValuePair<int, int> item in counts)
        {
            Guid sectionId = Guid.NewGuid();
            decimal score = QuestionTypeScores[item.Key];
            int count = Math.Min(item.Value, CountForSection);
            rules.Add(new PaperQuestionRule()
            {
                Id = Guid.NewGuid(),
                PaperSectionId = sectionId,
                TenantId = tenantId,
                QuestionBankId = questionBankId,
                QuestionType = item.Key,
                Count = count,
                Score = score,
            });
            sections.Add(new PaperSection()
            {
                Id = sectionId,
                PaperId = paperId,
                Title = QuestionTypeNames[item.Key],
                ScoreEach = QuestionTypeScores[item.Key],
                Order = sectionOrder++,
                TotalCount = count,
                TotalScore = count * score,
                TenantId = tenantId
            });
        }
        await connection.ExecuteAsync(
            @"INSERT INTO AppPapers (Id, Name, PaperType, Description, TotalQuestionCount, Score, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
            VALUES (@Id, @Name, 1, @Description, @TotalQuestionCount, @Score, NOW(), @TenantId, '{}', REPLACE(UUID(), '-', ''))", new Paper()
            {
                Id = paperId,
                Name = $"{bankName} 随机组卷",
                Description = "考试须知",
                TotalQuestionCount = sections.Sum(s => s.TotalCount),
                Score = sections.Sum(s => s.TotalScore),
                TenantId = tenantId
            });
        await connection.ExecuteAsync(
                @"INSERT INTO AppPaperSections (Id, PaperId, Title, ScoreEach, `Order`, TotalScore, TotalCount, CreationTime, TenantId)
                  VALUES (@Id, @PaperId, @Title, @ScoreEach, @Order, @TotalScore, @TotalCount, NOW(), @TenantId)", sections);
        await connection.ExecuteAsync(
                @"INSERT INTO AppPaperQuestionRules (Id, PaperSectionId, QuestionBankId, QuestionType, Count, Score, CreationTime, TenantId)
                  VALUES (@Id, @PaperSectionId, @QuestionBankId, @QuestionType, @Count, @Score, NOW(), @TenantId)", rules);
    }

    private async Task CreateFixedPaper(IDbConnection connection, Guid tenantId, Guid questionBankId, string bankName)
    {
        Guid paperId = Guid.NewGuid();

        List<dynamic> questions = (await connection.QueryAsync<dynamic>(@"SELECT Id, QuestionType FROM AppQuestions
                  WHERE QuestionBankId = @QuestionBankId
                  AND TenantId = @TenantId
                  ORDER BY CreationTime DESC", new
        {
            QuestionBankId = questionBankId,
            TenantId = tenantId
        })).ToList();
        List<dynamic> questionsTypes = questions.GroupBy(q => q.QuestionType).Select(q => q.Key).ToList();
        List<PaperSection> paperSections = [];
        List<PaperQuestion> paperQuestions = [];
        int sectionOrder = 1;
        foreach (dynamic questionsType in questionsTypes)
        {
            List<dynamic> currentQuestions = questions.Where(q => q.QuestionType == questionsType).Take(CountForSection).ToList();
            decimal sectionTotalScore = 0;
            int sectionTotalQuestionCount = 0;
            int questionOrder = 1;
            Guid sectionId = Guid.NewGuid();
            foreach (dynamic question in currentQuestions)
            {
                sectionTotalQuestionCount++;
                decimal score = QuestionTypeScores[questionsType];
                sectionTotalScore += score;
                paperQuestions.Add(new PaperQuestion()
                {
                    Id = Guid.NewGuid(),
                    PaperSectionId = sectionId,
                    QuestionId = question.Id,
                    Order = questionOrder++,
                    Score = score,
                    TenantId = tenantId
                });
            }
            paperSections.Add(new PaperSection()
            {
                Id = sectionId,
                PaperId = paperId,
                Title = QuestionTypeNames[questionsType],
                ScoreEach = QuestionTypeScores[questionsType],
                Order = sectionOrder++,
                TotalCount = sectionTotalQuestionCount,
                TotalScore = sectionTotalScore,
                TenantId = tenantId
            });
        }
        await connection.ExecuteAsync(@"INSERT INTO AppPapers (Id, Name, PaperType, Description, TotalQuestionCount, Score, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
            VALUES (@Id, @Name, 0, @Description, @TotalQuestionCount, @Score, NOW(), @TenantId, '{}', REPLACE(UUID(), '-', ''))", new Paper()
        {
            Id = paperId,
            Name = $"{bankName} 固定试卷",
            Description = "考试须知",
            TotalQuestionCount = paperSections.Sum(s => s.TotalCount),
            Score = paperSections.Sum(s => s.TotalScore),
            TenantId = tenantId
        });
        await connection.ExecuteAsync(
                @"INSERT INTO AppPaperSections (Id, PaperId, Title, ScoreEach, `Order`, TotalScore, TotalCount, CreationTime, TenantId)
                  VALUES (@Id, @PaperId, @Title, @ScoreEach, @Order, @TotalScore, @TotalCount, NOW(), @TenantId)", paperSections);
        await connection.ExecuteAsync(
                @"INSERT INTO AppPaperQuestions (Id, PaperSectionId, QuestionId, Score, `Order`, CreationTime, TenantId)
                  VALUES (@Id, @PaperSectionId, @QuestionId, @Score, @Order, NOW(), @TenantId)", paperQuestions);
    }

    private async Task CreateExamAsync(IDbConnection connection, Guid tenantId)
    {
        List<Paper> papers = (await connection.QueryAsync<Paper>(
            "SELECT * FROM AppPapers WHERE TenantId = @TenantId",
            new { TenantId = tenantId })).ToList();

        List<Examination> examinations = [];
        foreach (Paper paper in papers)
        {
            examinations.Add(new Examination()
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                Name = paper.Name + " 测试",
                Description = paper.Description,
                Score = paper.Score,
                PassingScore = paper.Score * 0.6m,
                TotalTime = 60,
                PaperId = paper.Id,
                Status = 1,
                AnswerMode = _rand.Next(0, 2),
                RandomOrderOfOption = true,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(7),
            });
        }
        await connection.ExecuteAsync(@"
            INSERT INTO AppExaminations (Id, Name, Description, Score, PassingScore, TotalTime, PaperId, Status,
                AnswerMode, RandomOrderOfOption, StartTime, EndTime, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp)
            VALUES (@Id, @Name, @Description, @Score, @PassingScore, @TotalTime, @PaperId, @Status, @AnswerMode,
                @RandomOrderOfOption, @StartTime, @EndTime, NOW(), @TenantId, '{}', REPLACE(UUID(), '-', ''))", examinations);
    }

    private async Task<bool> CreateQuestionAsync(IDbConnection connection, Guid tenantId)
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
                    answerParams.Add(new
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = qid,
                        Content = opt.Opt.Content,
                        Right = opt.Opt.Right,
                        Sort = opt.Index + 1,
                        CreationTime = DateTime.Now,
                        TenantId = tenantId
                    });
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
            await connection.ExecuteAsync("INSERT INTO AppQuestionAnswers (Id, QuestionId, Content, `Right`, Sort, CreationTime, TenantId) VALUES (@Id, @QuestionId, @Content, @Right, @Sort, @CreationTime, @TenantId)", answerParams);
        }
        if (knowledgepointParams.Count > 0)
        {
            await connection.ExecuteAsync("INSERT INTO AppQuestionKnowledgePoints (QuestionId, KnowledgePointId, CreationTime, TenantId, ExtraProperties, ConcurrencyStamp) VALUES (@QuestionId, @KnowledgePointId, @CreationTime, @TenantId, '{}', REPLACE(UUID(), '-', ''))", knowledgepointParams);
        }

        return true;
    }

    private async Task ClearDataAsync(IDbConnection connection, Guid tenantId)
    {
        await connection.ExecuteAsync("DELETE FROM AppPaperQuestions WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPaperQuestionRules WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPaperSections WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppPapers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppUserExamQuestions WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppUserExamSections WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppUserExams WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppKnowledgePoints WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppExaminations WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionAnswers WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestions WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionKnowledgePoints WHERE TenantId = @TenantId", new { TenantId = tenantId });
        await connection.ExecuteAsync("DELETE FROM AppQuestionBanks WHERE TenantId = @TenantId", new { TenantId = tenantId });
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

    public class Paper
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 总题数
        /// </summary>
        public int TotalQuestionCount { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; }

        public int PaperType { get; set; }
    }

    public class PaperSection
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PaperId { get; set; }
        public string Title { get; set; }
        public decimal ScoreEach { get; set; }
        public decimal TotalScore { get; set; }
        public int Order { get; set; }
        public int TotalCount { get; set; }
        public string? Remark { get; set; }
    }

    public class PaperQuestion
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid PaperSectionId { get; set; }
        public Guid QuestionId { get; set; }
        public int Order { get; set; }
        public decimal Score { get; set; }
    }

    public class PaperQuestionRule
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }

        /// <summary>
        /// 大题Id
        /// </summary>
        public Guid PaperSectionId { get; set; }

        /// <summary>
        /// 题库Id
        /// </summary>
        public Guid QuestionBankId { get; set; }

        public int QuestionType { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; }
    }

    public class Examination
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 及格分
        /// </summary>
        public decimal PassingScore { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public int TotalTime { get; set; }

        /// <summary>
        /// 试卷Id
        /// </summary>
        public Guid PaperId { get; set; }

        public int Status { get; set; }

        public int AnswerMode { get; set; }

        /// <summary>
        /// 选项乱序
        /// </summary>
        public bool RandomOrderOfOption { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public Guid? TenantId { get; set; }
    }
}