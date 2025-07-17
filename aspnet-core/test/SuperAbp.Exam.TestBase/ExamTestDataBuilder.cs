using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
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
    IPaperQuestionRuleRepository paperQuestionRuleRepository,
    IUserExamRepository userExamRepository,
    IUserExamQuestionRepository userExamQuestionRepository,
    ITrainingRepository trainingRepository,
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

            await CreatePaperQuestionRuleAsync();

            await CreateExamAsync();

            await CreateUserExamAsync();

            await CreateUserExamQuestionAsync();

            await CreateTrainingAsync();

            await CreateKnowledgePointAsync();
        }
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
        await trainingRepository.InsertManyAsync([
            new Training(testData.Training1Id, testData.User1Id, testData.QuestionBank1Id,
                testData.Question11Id, false, TrainingSource.QuestionBank),
            new Training(testData.Training2Id, testData.User1Id, testData.QuestionBank1Id,
                testData.Question11Id, false, TrainingSource.QuestionBank)
        ]);
    }

    private async Task CreateUserExamQuestionAsync()
    {
        await userExamQuestionRepository.InsertManyAsync([
            new UserExamQuestion(testData.UserExamQuestion111Id, testData.UserExam11Id, testData.Question11Id, 100),
            new UserExamQuestion(testData.UserExamQuestion112Id, testData.UserExam11Id, testData.Question12Id, 100),
            new UserExamQuestion(testData.UserExamQuestion113Id, testData.UserExam11Id, testData.Question13Id, 100),
            new UserExamQuestion(testData.UserExamQuestion114Id, testData.UserExam11Id, testData.Question14Id, 100),
            new UserExamQuestion(testData.UserExamQuestion121Id, testData.UserExam12Id, testData.Question11Id, 100),
            new UserExamQuestion(testData.UserExamQuestion122Id, testData.UserExam12Id, testData.Question12Id, 100),
            new UserExamQuestion(testData.UserExamQuestion123Id, testData.UserExam12Id, testData.Question13Id, 100),
            new UserExamQuestion(testData.UserExamQuestion124Id, testData.UserExam12Id, testData.Question14Id, 100),
            new UserExamQuestion(testData.UserExamQuestion211Id, testData.UserExam21Id, testData.Question11Id, 100),
            new UserExamQuestion(testData.UserExamQuestion212Id, testData.UserExam21Id, testData.Question12Id, 100),
            new UserExamQuestion(testData.UserExamQuestion213Id, testData.UserExam21Id, testData.Question13Id, 100),
            new UserExamQuestion(testData.UserExamQuestion214Id, testData.UserExam21Id, testData.Question14Id, 100),
            new UserExamQuestion(testData.UserExamQuestion221Id, testData.UserExam22Id, testData.Question11Id, 100),
            new UserExamQuestion(testData.UserExamQuestion222Id, testData.UserExam22Id, testData.Question12Id, 100),
            new UserExamQuestion(testData.UserExamQuestion223Id, testData.UserExam22Id, testData.Question13Id, 100),
            new UserExamQuestion(testData.UserExamQuestion224Id, testData.UserExam22Id, testData.Question14Id, 100),
        ]);
    }

    private async Task CreateUserExamAsync()
    {
        await userExamRepository.InsertManyAsync([
            new(testData.UserExam11Id, testData.Examination12Id, testData.User1Id)
            {
                Status = UserExamStatus.InProgress
            },
            new(testData.UserExam12Id, testData.Examination12Id, testData.User1Id)
            {
                Status = UserExamStatus.Submitted
            },
            new(testData.UserExam21Id, testData.Examination13Id, testData.User3Id)
            {
                Status = UserExamStatus.InProgress
            },
            new(testData.UserExam22Id, testData.Examination13Id, testData.User3Id)
            {
                Status = UserExamStatus.Submitted
            },
            new(testData.UserExam31Id, testData.Examination13Id, testData.User3Id),
        ]);
    }

    private async Task CreateExamAsync()
    {
        Examination ongoingExam = new(testData.Examination12Id, testData.Paper1Id,
            testData.Examination12Name, 100, 60, 60)
        {
            Status = ExaminationStatus.Published
        };
        Examination gradingExam = new(testData.Examination13Id, testData.Paper1Id,
            testData.Examination13Name, 100, 60, 60)
        {
            Status = ExaminationStatus.Grading
        };
        Examination completedExam = new(testData.Examination14Id, testData.Paper1Id,
            testData.Examination14Name, 100, 60, 60)
        {
            Status = ExaminationStatus.Completed
        };
        Examination cancelledExam = new(testData.Examination15Id, testData.Paper1Id,
            testData.Examination15Name, 100, 60, 60)
        {
            Status = ExaminationStatus.Cancelled
        };
        Examination timeExam = new(testData.Examination31Id, testData.Paper1Id,
            testData.Examination31Name, 100, 60, 60)
        {
            Status = ExaminationStatus.Published
        };
        timeExam.SetTime(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));
        await examRepository.InsertManyAsync([
            new Examination(testData.Examination11Id, testData.Paper1Id, testData.Examination11Name, 100, 60, 60),
            ongoingExam,
            gradingExam,
            completedExam,
            cancelledExam,
            new Examination(testData.Examination21Id, testData.Paper2Id, testData.Examination21Name, 100, 60, 60),
            new Examination(testData.Examination22Id, testData.Paper2Id, testData.Examination22Name, 100, 60, 60),
            timeExam
        ]);
    }

    private async Task CreatePaperQuestionRuleAsync()
    {
        await paperQuestionRuleRepository.InsertManyAsync([
            new PaperQuestionRule(testData.PaperQuestionRule1Id, testData.Paper1Id, testData.QuestionBank1Id)
            {
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
            new PaperQuestionRule(testData.PaperQuestionRule2Id, testData.Paper1Id, testData.QuestionBank1Id)
            {
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
        ]);
    }

    private async Task CreatePaperAsync()
    {
        await paperRepository.InsertManyAsync([
            new Paper(testData.Paper1Id, testData.Paper1Name, 100),
            new Paper(testData.Paper2Id, testData.Paper2Name, 100),
        ]);
    }

    private async Task CreateQuestionAsync()
    {
        await questionRepository.InsertManyAsync([
            new Question(testData.Question11Id, testData.QuestionBank1Id, QuestionType.SingleSelect, testData.Question11Content)
                .AddAnswer(testData.Answer111Id,testData.Answer111Content, false)
                .AddAnswer(testData.Answer112Id,testData.Answer112Content, true)
                .AddAnswer(testData.Answer113Id,testData.Answer113Content, false)
                .AddAnswer(testData.Answer114Id,testData.Answer114Content, false),
            new Question(testData.Question12Id, testData.QuestionBank1Id, QuestionType.MultiSelect, testData.Question12Content)
                .AddAnswer(testData.Answer121Id, testData.Answer121Content, false)
                .AddAnswer(testData.Answer122Id, testData.Answer122Content, true)
                .AddAnswer(testData.Answer123Id, testData.Answer123Content, true)
                .AddAnswer(testData.Answer124Id, testData.Answer124Content, false),
            new Question(testData.Question13Id, testData.QuestionBank1Id, QuestionType.Judge, testData.Question13Content)
                .AddAnswer(testData.Answer131Id,testData.Answer131Content, false)
                .AddAnswer(testData.Answer132Id,testData.Answer132Content, true),
            new Question(testData.Question14Id, testData.QuestionBank1Id, QuestionType.FillInTheBlanks, testData.Question14Content)
                .AddAnswer(testData.Answer141Id, testData.Answer141Content, true),
            new Question(testData.Question21Id, testData.QuestionBank2Id, QuestionType.SingleSelect, testData.Question21Content)
                .AddAnswer(testData.Answer211Id,testData.Answer211Content, false)
                .AddAnswer(testData.Answer212Id,testData.Answer212Content, true),
            new Question(testData.Question22Id, testData.QuestionBank2Id, QuestionType.MultiSelect, testData.Question22Content)
                .AddAnswer(testData.Answer221Id,testData.Answer221Content, false)
                .AddAnswer(testData.Answer222Id,testData.Answer222Content, true)
                .AddAnswer(testData.Answer223Id,testData.Answer223Content, false)
                .AddAnswer(testData.Answer224Id, testData.Answer224Content, false),
            new Question(testData.Question23Id, testData.QuestionBank2Id, QuestionType.Judge, testData.Question23Content)
                .AddAnswer(testData.Answer231Id,testData.Answer231Content, false)
                .AddAnswer(testData.Answer232Id,testData.Answer232Content, true),
            new Question(testData.Question24Id, testData.QuestionBank2Id, QuestionType.FillInTheBlanks, testData.Question24Content)
                .AddAnswer(testData.Answer241Id, testData.Answer241Content, true),
        ]);
    }

    private async Task CreateQuestionBankAsync()
    {
        await questionBankRepository.InsertManyAsync([
            new QuestionBank(testData.QuestionBank1Id, testData.QuestionBank1Title),
            new QuestionBank(testData.QuestionBank2Id, testData.QuestionBank2Title)]);
    }
}