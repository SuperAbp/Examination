using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SuperAbp.Exam.BackgroundServices.Sql;
using SuperAbp.Exam.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.BackgroundServices;

public class InitialDataWorker : AsyncPeriodicBackgroundWorkerBase
{
    public InitialDataWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 600_000;
#if DEBUG
        timer.RunOnStart = true;
#else
        timer.RunOnStart = true;
#endif
    }

    private PeriodicBackgroundWorkerContext _workerContext;
    private Guid _tenantId;
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

    [UnitOfWork(isTransactional: false)]
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        _workerContext = workerContext;
        IConfiguration configuration = workerContext.ServiceProvider.GetRequiredService<IConfiguration>();
        ILogger<InitialDataWorker> logger = workerContext.ServiceProvider.GetRequiredService<ILogger<InitialDataWorker>>();
        ITenantRepository tenantRepository = workerContext.ServiceProvider.GetRequiredService<ITenantRepository>();
        Tenant tenant = await tenantRepository.FindByNameAsync("Demo");
        _tenantId = tenant.Id;

        (ISqlProvider sqlProvider, IDbConnection connection) = GetSqlProvider(configuration);
        if (!await CheckTimeAsync(configuration, logger, sqlProvider, connection))
        {
            return;
        }

#if DEBUG
#else
        if (!int.TryParse(configuration["InitialData:TargetHour"], out int targetHour))
        {
            targetHour = 1;
        }
        if (DateTime.Now.Hour != targetHour)
        {
            return;
        }
#endif

        logger.LogDebug("Clear data……");
        ICurrentTenant currentTenant = _workerContext.ServiceProvider.GetRequiredService<ICurrentTenant>();
        using (currentTenant.Change(_tenantId))
        {
            await ClearDataAsync();
            logger.LogDebug("Create Question……");
            bool flowControl = await CreateQuestionAsync();
            if (!flowControl)
            {
                return;
            }
            logger.LogDebug("Create Paper……");
            await CreatePaperAsync();
            logger.LogDebug("Create Exam……");
            await CreateExamAsync();
            logger.LogDebug("Create Record……");
            await connection.ExecuteAsync(sqlProvider.InsertInitialDataExecutionLog(), new { LastExecutedTime = DateTime.Now });
            logger.LogDebug("Created successfully.");
        }
    }

    private static (ISqlProvider, IDbConnection) GetSqlProvider(IConfiguration configuration)
    {
        ISqlProvider sqlProvider;
        IDbConnection connection;
        if (configuration["DatabaseType"] == "mysql")
        {
            sqlProvider = new MySqlProvider();
            connection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }
        else
        {
            sqlProvider = new SqlServerProvider();
            connection = new SqlConnection(configuration.GetConnectionString("Default"));
        }
        return (sqlProvider, connection);
    }

    private static async Task<bool> CheckTimeAsync(IConfiguration configuration, ILogger<InitialDataWorker> logger, ISqlProvider sqlProvider, IDbConnection connection)
    {
        // TODO:Remove InitialDataExecutionLog Table
        DateTime lastExecutedTime = await connection.ExecuteScalarAsync<DateTime>(sqlProvider.GetLastExecutedTime());
        if (!int.TryParse(configuration["InitialData:IntervalDays"], out int intervalDays))
        {
            intervalDays = 1;
        }
        if ((Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00") - Convert.ToDateTime(lastExecutedTime.ToString("yyyy-MM-dd") + " 00:00:00")).Days < intervalDays)
        {
            logger.LogInformation("Initial data has already been executed today, skipping.");
            return false;
        }

        return true;
    }

    private async Task CreatePaperAsync()
    {
        var paperRepository = _workerContext.ServiceProvider.GetRequiredService<IPaperRepository>();
        IQuestionBankRepository questionBankRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionBankRepository>();
        var questionBanks = await questionBankRepository.GetListAsync();

        List<Paper> papers = [];
        foreach (var questionBank in questionBanks)
        {
            Guid questionBankId = questionBank.Id;
            string bankName = questionBank.Title ?? "";

            papers.Add(await CreateRandomPaper(questionBankId, bankName));
            papers.Add(await CreateFixedPaper(questionBankId, bankName));
        }
        await paperRepository.InsertManyAsync(papers, true);
    }

    private async Task<Paper> CreateRandomPaper(Guid questionBankId, string bankName)
    {
        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();
        var questionRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionRepository>();
        var paperManager = _workerContext.ServiceProvider.GetRequiredService<PaperManager>();

        Dictionary<QuestionType, int> counts = await (await questionRepository.GetQueryableAsync())
            .GroupBy(q => q.QuestionType)
            .Select(c => new { key = c.Key, Count = c.Count() })
            .ToDictionaryAsync(g => g.key, g => g.Count);
        int sectionOrder = 1;
        Paper paper = await paperManager.CreateAsync(PaperType.Random, $"{bankName} 随机组卷", false);
        paper.Description = "考试须知";
        foreach (KeyValuePair<QuestionType, int> item in counts)
        {
            Guid sectionId = guidGenerator.Create();
            decimal score = QuestionTypeScores[item.Key];
            int count = Math.Min(item.Value, CountForSection);
            paper.AddSection(sectionId, QuestionTypeNames[item.Key], QuestionTypeScores[item.Key], sectionOrder++);
            paper.AddRule(sectionId, guidGenerator.Create(), questionBankId, item.Key, count, score);
        }
        return paper;
    }

    private async Task<Paper> CreateFixedPaper(Guid questionBankId, string bankName)
    {
        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();
        var questionRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionRepository>();
        var paperManager = _workerContext.ServiceProvider.GetRequiredService<PaperManager>();
        var questions = await (await questionRepository.GetQueryableAsync()).Where(q => q.QuestionBankId == questionBankId).ToListAsync();
        var questionsTypes = questions.GroupBy(q => q.QuestionType).Select(q => q.Key).ToList();

        Paper paper = await paperManager.CreateAsync(PaperType.Fixed, $"{bankName} 固定试卷", false);
        paper.Description = "考试须知";

        int sectionOrder = 1;
        foreach (var questionsType in questionsTypes)
        {
            var currentQuestions = questions.Where(q => q.QuestionType == questionsType).Take(CountForSection).ToList();
            int questionOrder = 1;
            Guid sectionId = guidGenerator.Create();
            paper.AddSection(sectionId, QuestionTypeNames[questionsType], QuestionTypeScores[questionsType], sectionOrder++);

            foreach (var question in currentQuestions)
            {
                paper.AddQuestion(sectionId, guidGenerator.Create(), question.Id, QuestionTypeScores[questionsType], questionOrder++);
            }
        }

        return paper;
    }

    private async Task CreateExamAsync()
    {
        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();
        var paperRepository = _workerContext.ServiceProvider.GetRequiredService<IPaperRepository>();
        var examRepository = _workerContext.ServiceProvider.GetRequiredService<IExamRepository>();
        var papers = await paperRepository.GetListAsync();

        List<Examination> examinations = [];
        foreach (Paper paper in papers)
        {
            Examination examination = new(guidGenerator.Create(), paper.Id, paper.Name + " 测试", paper.Score,
                paper.Score * 0.6m, 60, AnswerMode.FromValue(_rand.Next(0, 2)), true, false, ReviewMode.FromValue(_rand.Next(0, 2)));
            examination.SetTime(DateTime.Now, DateTime.Now.AddDays(7));
            examinations.Add(examination);
        }
        await examRepository.InsertManyAsync(examinations);
    }

    private async Task<bool> CreateQuestionAsync()
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

        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();
        var questionBankRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionBankRepository>();
        var questionBankManager = _workerContext.ServiceProvider.GetRequiredService<QuestionBankManager>();
        var questionManager = _workerContext.ServiceProvider.GetRequiredService<QuestionManager>();
        var questionRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionRepository>();
        var questionKnowledgePointRepository = _workerContext.ServiceProvider.GetRequiredService<IQuestionKnowledgePointRepository>();
        List<QuestionBank> questionBanks = [];
        List<Question> questions = [];
        List<QuestionOption> options = [];
        List<QuestionKnowledgePoint> knowledgePoints = [];

        Question tempQuestion;
        QuestionBank tempBank;
        var bankParams = new List<object>();
        var questionParams = new List<object>();
        var answerParams = new List<object>();
        var knowledgepointParams = new List<object>(); // question-knowledgepoint relations

        var seenKnowledgePoints = new HashSet<string>();

        foreach (var bank in doc.QuesiontBanks)
        {
            tempBank = await questionBankManager.CreateAsync(bank.Name ?? String.Empty);
            questionBanks.Add(tempBank);

            if (bank.Questions == null) continue;

            foreach (var q in bank.Questions)
            {
                tempQuestion = await questionManager.CreateAsync(tempBank.Id, QuestionType.FromValue(q.Type), q.Title ?? string.Empty);
                questions.Add(tempQuestion);

                if (q.KnowledgePoints != null)
                {
                    var newKps = q.KnowledgePoints.Except(seenKnowledgePoints).ToList();
                    foreach (var kpId in newKps)
                    {
                        seenKnowledgePoints.Add(kpId);
                    }

                    foreach (var kpId in q.KnowledgePoints)
                    {
                        if (kpId is null)
                        {
                            continue;
                        }
                        knowledgePoints.Add(new QuestionKnowledgePoint(tempQuestion.Id, new Guid(kpId)));
                    }
                }

                if (q.Options == null) continue;

                foreach (var opt in q.Options.Select((o, idx) => new { Opt = o, Index = idx }))
                {
                    tempQuestion.AddOption(guidGenerator.Create(), opt.Opt.Content ?? String.Empty, opt.Opt.Right, opt.Index + 1);
                }
            }
        }
        if (questionBanks.Count > 0)
        {
            await questionBankRepository.InsertManyAsync(questionBanks, true);
        }
        if (questions.Count > 0)
        {
            await questionRepository.InsertManyAsync(questions, true);
        }
        if (knowledgePoints.Count > 0)
        {
            await questionKnowledgePointRepository.InsertManyAsync(knowledgePoints);
        }

        return true;
    }

    private async Task ClearDataAsync()
    {
        ExamDbContext dbContext = _workerContext.ServiceProvider.GetRequiredService<ExamDbContext>();
        await dbContext.PaperQuestions.ExecuteDeleteAsync();
        await dbContext.PaperQuestionRules.ExecuteDeleteAsync();
        await dbContext.PaperSections.ExecuteDeleteAsync();
        await dbContext.Papers.ExecuteDeleteAsync();
        await dbContext.UserExamQuestionReviews.ExecuteDeleteAsync();
        await dbContext.UerExamQuestions.ExecuteDeleteAsync();
        await dbContext.UserExamSections.ExecuteDeleteAsync();
        await dbContext.UserExams.ExecuteDeleteAsync();
        await dbContext.Exams.ExecuteDeleteAsync();
        await dbContext.QuestionOptions.ExecuteDeleteAsync();
        await dbContext.Questions.ExecuteDeleteAsync();
        await dbContext.QuestionKnowledgePoints.ExecuteDeleteAsync();
        await dbContext.QuestionBanks.ExecuteDeleteAsync();
        await dbContext.KnowledgePoints.ExecuteDeleteAsync();
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