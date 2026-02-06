using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam;

public class ExamTestDataSeedContributor(ICurrentTenant currentTenant,
    IQuestionRepository questionRepository,
    IQuestionBankRepository questionBankRepository,
    IKnowledgePointRepository knowledgePointRepository,
    IExamRepository examRepository,
    IPaperRepository paperRepository,
    ITrainingRepository trainingRepository,
    IAnnouncementRepository announcementRepository,
    IAnnouncementCategoryRepository announcementCategoryRepository,
    ExamTestData testData) : IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        using (currentTenant.Change(context?.TenantId))
        {
            await CreateQuestionBankAsync();

            await CreateQuestionAsync();

            await CreatePaperAsync();

            await CreateExamAsync();

            await CreateTrainingAsync();

            await CreateKnowledgePointAsync();

            await CreateAnnouncementCategoryAsync();

            await CreateAnnouncementAsync();
        }
    }

    private async Task CreatePaperAsync()
    {
        await paperRepository.InsertManyAsync([
            new Paper(testData.Paper1Id, PaperType.Fixed, testData.Paper1Name, false),
            new Paper(testData.Paper2Id, PaperType.Fixed, testData.Paper2Name, false),
        ]);
    }

    private async Task CreateKnowledgePointAsync()
    {
        await knowledgePointRepository.InsertManyAsync([
            new KnowledgePoint(testData.KnowledgePoint1Id, testData.KnowledgePoint1Name),
            new KnowledgePoint(testData.KnowledgePoint11Id, testData.KnowledgePoint11Name, testData.KnowledgePoint1Id),
            new KnowledgePoint(testData.KnowledgePoint2Id, testData.KnowledgePoint2Name)
        ]);
    }

    private async Task CreateTrainingAsync()
    {
        Training training1 = new Training(testData.Training1Id, testData.User1Id, testData.QuestionBank1Id,
            testData.Question11Id, TrainingSource.QuestionBank);
        Training training2 = new Training(testData.Training2Id, testData.User1Id, testData.QuestionBank1Id,
            testData.Question11Id, TrainingSource.QuestionBank);
        await trainingRepository.InsertManyAsync([training1, training2]);
    }

    private async Task CreateExamAsync()
    {
        Examination ongoingExam = new(testData.Examination12Id, testData.Paper1Id,
            testData.Examination12Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified)
        {
            Status = ExaminationStatus.Published
        };
        Examination gradingExam = new(testData.Examination13Id, testData.Paper1Id,
            testData.Examination13Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified)
        {
            Status = ExaminationStatus.Grading
        };
        Examination completedExam = new(testData.Examination14Id, testData.Paper1Id,
            testData.Examination14Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified)
        {
            Status = ExaminationStatus.Completed
        };
        Examination cancelledExam = new(testData.Examination15Id, testData.Paper1Id,
            testData.Examination15Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified)
        {
            Status = ExaminationStatus.Cancelled
        };
        Examination timeExam = new(testData.Examination31Id, testData.Paper1Id,
            testData.Examination31Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.RealTime)
        {
            Status = ExaminationStatus.Published
        };
        timeExam.SetTime(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));
        await examRepository.InsertManyAsync([
            new Examination(testData.Examination11Id, testData.Paper1Id, testData.Examination11Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified),
            ongoingExam,
            gradingExam,
            completedExam,
            cancelledExam,
            new Examination(testData.Examination21Id, testData.Paper2Id, testData.Examination21Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.Unified),
            new Examination(testData.Examination22Id, testData.Paper2Id, testData.Examination22Name, 100, 60, 60, AnswerMode.All, false, true, ReviewMode.RealTime),
            timeExam
        ]);
    }

    private async Task CreateQuestionAsync()
    {
        await questionRepository.InsertManyAsync([
            new Question(testData.Question11Id, testData.QuestionBank1Id, QuestionType.SingleSelect, testData.Question11Content)
                .AddOption(testData.Answer111Id,testData.Answer111Content, false)
                .AddOption(testData.Answer112Id,testData.Answer112Content, true)
                .AddOption(testData.Answer113Id,testData.Answer113Content, false)
                .AddOption(testData.Answer114Id,testData.Answer114Content, false),
            new Question(testData.Question12Id, testData.QuestionBank1Id, QuestionType.MultiSelect, testData.Question12Content)
                .AddOption(testData.Answer121Id, testData.Answer121Content, false)
                .AddOption(testData.Answer122Id, testData.Answer122Content, true)
                .AddOption(testData.Answer123Id, testData.Answer123Content, true)
                .AddOption(testData.Answer124Id, testData.Answer124Content, false),
            new Question(testData.Question13Id, testData.QuestionBank1Id, QuestionType.Judge, testData.Question13Content)
                .AddOption(testData.Answer131Id,testData.Answer131Content, false)
                .AddOption(testData.Answer132Id,testData.Answer132Content, true),
            new Question(testData.Question14Id, testData.QuestionBank1Id, QuestionType.FillInTheBlanks, testData.Question14Content)
                .AddOption(testData.Answer141Id, testData.Answer141Content, true),
            new Question(testData.Question21Id, testData.QuestionBank2Id, QuestionType.SingleSelect, testData.Question21Content)
                .AddOption(testData.Answer211Id,testData.Answer211Content, false)
                .AddOption(testData.Answer212Id,testData.Answer212Content, true),
            new Question(testData.Question22Id, testData.QuestionBank2Id, QuestionType.MultiSelect, testData.Question22Content)
                .AddOption(testData.Answer221Id,testData.Answer221Content, false)
                .AddOption(testData.Answer222Id,testData.Answer222Content, true)
                .AddOption(testData.Answer223Id,testData.Answer223Content, false)
                .AddOption(testData.Answer224Id, testData.Answer224Content, false),
            new Question(testData.Question23Id, testData.QuestionBank2Id, QuestionType.Judge, testData.Question23Content)
                .AddOption(testData.Answer231Id,testData.Answer231Content, false)
                .AddOption(testData.Answer232Id,testData.Answer232Content, true),
            new Question(testData.Question24Id, testData.QuestionBank2Id, QuestionType.FillInTheBlanks, testData.Question24Content)
                .AddOption(testData.Answer241Id, testData.Answer241Content, true),
        ]);
    }

    private async Task CreateQuestionBankAsync()
    {
        await questionBankRepository.InsertManyAsync([
            new QuestionBank(testData.QuestionBank1Id, testData.QuestionBank1Title),
            new QuestionBank(testData.QuestionBank2Id, testData.QuestionBank2Title)]);
    }

    private async Task CreateAnnouncementCategoryAsync()
    {
        await announcementCategoryRepository.InsertManyAsync([
            new AnnouncementCategory(testData.AnnouncementCategory1Id, testData.AnnouncementCategory1Name, 1, "系统相关公告"),
            new AnnouncementCategory(testData.AnnouncementCategory2Id, testData.AnnouncementCategory2Name, 2, "活动相关通知")
        ]);
    }

    private async Task CreateAnnouncementAsync()
    {
        var announcement1 = new Announcement(
            testData.Announcement1Id,
            testData.Announcement1Title,
            testData.Announcement1Content,
            1,
            testData.AnnouncementCategory1Id
        );
        announcement1.PublishTime = DateTime.Now.AddDays(-1);
        announcement1.Publish();

        var announcement2 = new Announcement(
            testData.Announcement2Id,
            testData.Announcement2Title,
            testData.Announcement2Content,
            2,
            testData.AnnouncementCategory2Id
        );
        announcement2.PublishTime = DateTime.Now;
        announcement2.Publish();

        var announcement3 = new Announcement(
            testData.Announcement3Id,
            testData.Announcement3Title,
            testData.Announcement3Content,
            3,
            testData.AnnouncementCategory1Id
        );
        announcement3.PublishTime = DateTime.Now.AddHours(-2);
        announcement3.Publish();

        var announcement4 = new Announcement(
            testData.Announcement4Id,
            testData.Announcement4Title,
            testData.Announcement4Content,
            4,
            testData.AnnouncementCategory2Id
        );

        await announcementRepository.InsertManyAsync([announcement1, announcement2, announcement3, announcement4]);
    }
}