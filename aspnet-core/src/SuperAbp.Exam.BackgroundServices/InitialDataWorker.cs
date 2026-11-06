using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Npgsql;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.BackgroundServices.Sql;
using SuperAbp.Exam.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;
using SuperAbp.Exam.Notifications;
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
    public InitialDataWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 600_000;
#if DEBUG
        timer.RunOnStart = configuration.GetValue("InitialData:RunOnStart", false);
#else
        timer.RunOnStart = true;
#endif
    }

    private PeriodicBackgroundWorkerContext _workerContext;
    private Guid? _tenantId;
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
        string tenantName = configuration["InitialData:TenantName"];
        if (!string.IsNullOrWhiteSpace(tenantName))
        {
            Tenant tenant = await tenantRepository.FindByNameAsync(tenantName);
            _tenantId = tenant.Id;
        }

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
            logger.LogDebug("Create Announcement……");
            await CreateAnnouncementsAsync();
            logger.LogDebug("Create Notification……");
            await CreateNotificationsAsync(logger);
            logger.LogDebug("Create Record……");
            try
            {
                await connection.ExecuteAsync(sqlProvider.InsertInitialDataExecutionLog(), new { LastExecutedTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to write InitialDataExecutionLog, ignore.");
            }
            logger.LogDebug("Created successfully.");
        }
    }

    private static (ISqlProvider, IDbConnection) GetSqlProvider(IConfiguration configuration)
    {
        ISqlProvider sqlProvider;
        IDbConnection connection;
        string? databaseType = configuration["DatabaseType"]?.ToLower();
        if (databaseType == "mysql")
        {
            sqlProvider = new MySqlProvider();
            connection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }
        else if (databaseType == "postgresql" || databaseType == "postgres")
        {
            sqlProvider = new PostgresProvider();
            connection = new NpgsqlConnection(configuration.GetConnectionString("Default"));
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
        DateTime lastExecutedTime;
        try
        {
            lastExecutedTime = await connection.ExecuteScalarAsync<DateTime>(sqlProvider.GetLastExecutedTime());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to read InitialDataExecutionLog, will run initial data directly.");
            return true;
        }
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

    private async Task CreateAnnouncementsAsync()
    {
        var categoryRepository = _workerContext.ServiceProvider.GetRequiredService<IAnnouncementCategoryRepository>();
        var announcementRepository = _workerContext.ServiceProvider.GetRequiredService<IAnnouncementRepository>();
        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();

        AnnouncementCategory examCategory = new(guidGenerator.Create(), "考试通知", 0, "考试安排相关公告");
        AnnouncementCategory systemCategory = new(guidGenerator.Create(), "系统公告", 10, "平台功能与维护公告");

        DateTime now = DateTime.Now;
        List<Announcement> announcements =
        [
            new(guidGenerator.Create(), "下半年考试安排通知", "2026年下半年统一考试将于下月开启报名，请各位学员及时关注报名截止时间，并提前完成考前复习。", 0, examCategory.Id) { IsPublished = true },
            new(guidGenerator.Create(), "系统维护公告", "平台将于本周六凌晨 2:00-4:00 进行系统升级维护，期间暂停访问，请合理安排学习时间。", 0, systemCategory.Id) { IsPublished = true },
            new(guidGenerator.Create(), "模拟考试功能上线", "模拟考试功能已上线，支持随机组卷与固定试卷两种模式，欢迎体验。", 1, systemCategory.Id) { IsPublished = true, ScheduledExpirationTime = now.AddDays(30) },
            new(guidGenerator.Create(), "新题库发布预告", "信息安全基础题库将于近期发布，敬请期待。", 2, examCategory.Id),
            new(guidGenerator.Create(), "节假日放假安排", "根据国家法定节假日安排，平台客服将在假期暂停服务，考试与学习功能不受影响。", 3, systemCategory.Id),
        ];

        Announcement scheduled = new(guidGenerator.Create(), "限时优惠活动公告", "为庆祝平台注册学员突破 10000 人，本月报名考试享受 8 折优惠。", 4, examCategory.Id);
        scheduled.SetPublishTime(now.AddMinutes(1));
        announcements.Add(scheduled);

        Announcement expired = new(guidGenerator.Create(), "旧版客户端停用提醒", "旧版客户端已停止服务，请下载最新版客户端。", 5, systemCategory.Id) { IsPublished = true, ScheduledExpirationTime = now.AddDays(-1) };
        announcements.Add(expired);

        await categoryRepository.InsertManyAsync([examCategory, systemCategory]);
        await announcementRepository.InsertManyAsync(announcements);
    }

    private async Task CreateNotificationsAsync(ILogger logger)
    {
        ExamDbContext dbContext = _workerContext.ServiceProvider.GetRequiredService<ExamDbContext>();
        Guid? adminUserId = await dbContext.Users
            .Where(user => user.NormalizedUserName == "ADMIN")
            .Select(user => (Guid?)user.Id)
            .FirstOrDefaultAsync();
        if (adminUserId is null)
        {
            logger.LogWarning("Admin user not found, skip creating notifications.");
            return;
        }

        var notificationRepository = _workerContext.ServiceProvider.GetRequiredService<INotificationRepository>();
        var guidGenerator = _workerContext.ServiceProvider.GetRequiredService<IGuidGenerator>();

        List<Notification> notifications =
        [
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.ExamStartReminder, JsonSerializer.Serialize(new { message = "您预约的《驾考科目一 固定试卷 测试》将于 1 小时后开始，请提前做好准备。" })),
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.ExamEndReminder, JsonSerializer.Serialize(new { message = "《二级建造师 随机组卷 测试》将于 30 分钟后结束，请尽快提交答卷。" })),
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.ExamScorePublished, JsonSerializer.Serialize(new { examName = "驾考科目一 固定试卷 测试" })),
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.CommentReply, JsonSerializer.Serialize(new { message = "您的提问有新的回复：填空题支持多个可选答案，多个答案使用 || 分隔。" })),
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.QuestionApproved, JsonSerializer.Serialize(new { message = "您提交的题目已通过审核。" })),
            new(guidGenerator.Create(), adminUserId.Value, NotificationType.TrainingUpdated, JsonSerializer.Serialize(new { message = "《安全生产培训》课程内容已更新，可前往查看。" })),
        ];
        notifications[0].MarkAsRead(DateTime.Now);
        notifications[2].MarkAsRead(DateTime.Now);

        await notificationRepository.InsertManyAsync(notifications);
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
        var knowledgePointRepository = _workerContext.ServiceProvider.GetRequiredService<IKnowledgePointRepository>();
        List<QuestionBank> questionBanks = [];
        List<Question> questions = [];
        List<QuestionOption> options = [];
        List<QuestionKnowledgePoint> knowledgePoints = [];
        List<KnowledgePoint> knowledgePointList = [];
        Dictionary<string, KnowledgePoint> knowledgePointMap = new(StringComparer.Ordinal);

        foreach (var bank in doc.QuesiontBanks)
        {
            QuestionBank tempBank = await questionBankManager.CreateAsync(bank.Name ?? String.Empty);
            questionBanks.Add(tempBank);

            if (bank.Questions == null) continue;

            foreach (var q in bank.Questions)
            {
                Question tempQuestion = await questionManager.CreateAsync(tempBank.Id, QuestionType.FromValue(q.Type), q.Title ?? string.Empty);
                tempQuestion.Analysis = q.Analysis;
                tempQuestion.FixedOrder = q.FixedOrder;
                tempQuestion.RequiredAnswerCount = q.RequiredAnswerCount;
                questions.Add(tempQuestion);

                if (q.KnowledgePoints != null)
                {
                    foreach (string knowledgePointPath in q.KnowledgePoints)
                    {
                        if (string.IsNullOrWhiteSpace(knowledgePointPath))
                        {
                            continue;
                        }
                        KnowledgePoint? knowledgePoint = GetOrCreateKnowledgePoint(knowledgePointPath.Trim(), knowledgePointMap, knowledgePointList, guidGenerator);
                        if (knowledgePoint is null)
                        {
                            continue;
                        }
                        if (!knowledgePoints.Any(x => x.QuestionId == tempQuestion.Id && x.KnowledgePointId == knowledgePoint.Id))
                        {
                            knowledgePoints.Add(new QuestionKnowledgePoint(tempQuestion.Id, knowledgePoint.Id));
                        }
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
        if (knowledgePointList.Count > 0)
        {
            await knowledgePointRepository.InsertManyAsync(knowledgePointList, true);
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

    private static KnowledgePoint? GetOrCreateKnowledgePoint(
        string knowledgePointPath,
        Dictionary<string, KnowledgePoint> knowledgePointMap,
        List<KnowledgePoint> knowledgePointList,
        IGuidGenerator guidGenerator)
    {
        Guid? parentId = null;
        string key = string.Empty;
        KnowledgePoint? knowledgePoint = null;
        foreach (string name in knowledgePointPath.Split(" > "))
        {
            string trimmedName = name.Trim();
            if (trimmedName.Length == 0)
            {
                continue;
            }
            key = key.Length == 0 ? trimmedName : key + " > " + trimmedName;
            if (!knowledgePointMap.TryGetValue(key, out knowledgePoint))
            {
                knowledgePoint = new KnowledgePoint(guidGenerator.Create(), trimmedName, parentId);
                knowledgePointMap[key] = knowledgePoint;
                knowledgePointList.Add(knowledgePoint);
            }
            parentId = knowledgePoint.Id;
        }
        return knowledgePoint;
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
        await dbContext.Notifications.ExecuteDeleteAsync();
        await dbContext.Announcements.ExecuteDeleteAsync();
        await dbContext.AnnouncementCategories.ExecuteDeleteAsync();
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
        public bool FixedOrder { get; set; }
        public int RequiredAnswerCount { get; set; }
        public string[] KnowledgePoints { get; set; } = [];
        public List<OptionDto>? Options { get; set; }
    }

    public class OptionDto
    {
        public string? Content { get; set; }
        public bool Right { get; set; }
    }
}
