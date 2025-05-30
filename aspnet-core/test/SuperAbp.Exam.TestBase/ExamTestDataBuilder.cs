using System;
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
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam;

public class ExamTestDataSeedContributor(ICurrentTenant currentTenant,
    IQuestionRepository questionRepository,
    IQuestionBankRepository questionBankRepository,
    IQuestionAnswerRepository questionAnswerRepository,
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
            await questionBankRepository.InsertManyAsync([
                new QuestionBank(testData.QuestionBank1Id, testData.QuestionBank1Title),
                new QuestionBank(testData.QuestionBank2Id, testData.QuestionBank2Title)]);

            await questionRepository.InsertManyAsync([
                new Question(testData.Question11Id, testData.QuestionBank1Id, QuestionType.SingleSelect, testData.Question11Content1),
                new Question(testData.Question12Id, testData.QuestionBank1Id, QuestionType.SingleSelect, testData.Question12Content2),
                new Question(testData.Question21Id, testData.QuestionBank2Id, QuestionType.SingleSelect, testData.Question21Content1),
                new Question(testData.Question22Id, testData.QuestionBank2Id, QuestionType.SingleSelect, testData.Question22Content2),
                ]);

            await questionAnswerRepository.InsertManyAsync([
                new QuestionAnswer(testData.Answer111Id, testData.Question11Id, testData.Answer111Content, false),
                new QuestionAnswer(testData.Answer112Id, testData.Question11Id, testData.Answer112Content, true),
                new QuestionAnswer(testData.Answer113Id, testData.Question11Id, testData.Answer113Content, false),
                new QuestionAnswer(testData.Answer114Id, testData.Question11Id, testData.Answer114Content, false),
                new QuestionAnswer(testData.Answer121Id, testData.Question12Id, testData.Answer121Content, false),
                new QuestionAnswer(testData.Answer122Id, testData.Question12Id, testData.Answer122Content, true),
                new QuestionAnswer(testData.Answer123Id, testData.Question12Id, testData.Answer123Content, false),
                new QuestionAnswer(testData.Answer124Id, testData.Question12Id, testData.Answer124Content, false),
                new QuestionAnswer(testData.Answer211Id, testData.Question21Id, testData.Answer211Content, false),
                new QuestionAnswer(testData.Answer212Id, testData.Question21Id, testData.Answer212Content, true),
                new QuestionAnswer(testData.Answer213Id, testData.Question21Id, testData.Answer213Content, false),
                new QuestionAnswer(testData.Answer214Id, testData.Question21Id, testData.Answer214Content, false),
                new QuestionAnswer(testData.Answer221Id, testData.Question22Id, testData.Answer221Content, false),
                new QuestionAnswer(testData.Answer222Id, testData.Question22Id, testData.Answer222Content, true),
                new QuestionAnswer(testData.Answer223Id, testData.Question22Id, testData.Answer223Content, false),
                new QuestionAnswer(testData.Answer224Id, testData.Question22Id, testData.Answer224Content, false)]);

            await paperRepository.InsertManyAsync([
                new Paper(testData.Paper1Id, testData.Paper1Name, 100),
                new Paper(testData.Paper2Id, testData.Paper2Name, 100),
            ]);

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

            Examination ongoingExam = new(testData.Examination12Id, testData.Paper1Id,
                testData.Examination12Name, 100, 60, 60)
            {
                Status = ExaminationStatus.Ongoing
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
                Status = ExaminationStatus.Ongoing
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

            UserExam inProgressUserExam = new(testData.UserExam31Id, testData.Examination11Id, testData.User2Id)
            {
                Status = UserExamStatus.InProgress
            };
            UserExam submittedUserExam = new(testData.UserExam13Id, testData.Examination13Id, testData.User1Id)
            {
                Status = UserExamStatus.Submitted
            };
            await userExamRepository.InsertManyAsync([
                new UserExam(testData.UserExam11Id, testData.Examination11Id, testData.User1Id),
                new UserExam(testData.UserExam12Id, testData.Examination12Id, testData.User1Id),
                new UserExam(testData.UserExam21Id, testData.Examination21Id, testData.User1Id),
                new UserExam(testData.UserExam22Id, testData.Examination22Id, testData.User1Id),
                inProgressUserExam,
                submittedUserExam
            ]);

            await userExamQuestionRepository.InsertManyAsync([
                new UserExamQuestion(testData.UserExamQuestion11Id, testData.UserExam11Id, testData.Question11Id, 100),
                new UserExamQuestion(testData.UserExamQuestion12Id, testData.UserExam11Id, testData.Question12Id, 100),
                new UserExamQuestion(testData.UserExamQuestion13Id, testData.UserExam12Id, testData.Question11Id, 100),
                new UserExamQuestion(testData.UserExamQuestion14Id, testData.UserExam12Id, testData.Question12Id, 100),
                new UserExamQuestion(testData.UserExamQuestion15Id, testData.UserExam13Id, testData.Question11Id, 100),
                new UserExamQuestion(testData.UserExamQuestion16Id, testData.UserExam13Id, testData.Question12Id, 100),
                new UserExamQuestion(testData.UserExamQuestion21Id, testData.UserExam21Id, testData.Question11Id, 100),
                new UserExamQuestion(testData.UserExamQuestion22Id, testData.UserExam21Id, testData.Question12Id, 100)
            ]);

            await trainingRepository.InsertManyAsync([
                new Training(testData.Training1Id, testData.User1Id, testData.QuestionBank1Id,
                    testData.Question11Id, false, TrainingSource.QuestionBank),
                new Training(testData.Training2Id, testData.User1Id, testData.QuestionBank1Id,
                    testData.Question11Id, false, TrainingSource.QuestionBank)
            ]);

            await knowledgePointRepository.InsertManyAsync([
                new KnowledgePoint(testData.KnowledgePoint1Id, testData.KnowledgePoint1Name),
                new KnowledgePoint(testData.KnowledgePoint11Id, testData.KnowledgePoint11Name, testData.KnowledgePoint1Id),
                new KnowledgePoint(testData.KnowledgePoint2Id, testData.KnowledgePoint2Name)
            ]);
        }
    }
}