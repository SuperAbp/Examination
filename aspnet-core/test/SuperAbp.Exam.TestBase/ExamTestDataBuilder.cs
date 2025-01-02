using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam;

public class ExamTestDataSeedContributor(ICurrentTenant currentTenant,
    IQuestionRepository questionRepository,
    IQuestionRepoRepository questionRepoRepository,
    IQuestionAnswerRepository questionAnswerRepository,
    ExamTestData testData) : IDataSeedContributor, ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        /* Seed additional test data... */

        using (currentTenant.Change(context?.TenantId))
        {
            await questionRepoRepository.InsertManyAsync([
                new QuestionRepo(testData.QuestionRepository1Id, testData.QuestionRepository1Title),
                new QuestionRepo(testData.QuestionRepository2Id, testData.QuestionRepository2Title)]);

            await questionRepository.InsertManyAsync([
                new Question(testData.Question11Id, testData.QuestionRepository1Id, QuestionType.SingleSelect, testData.Question11Content1),
                new Question(testData.Question12Id, testData.QuestionRepository1Id, QuestionType.SingleSelect, testData.Question12Content2),
                new Question(testData.Question21Id, testData.QuestionRepository2Id, QuestionType.SingleSelect, testData.Question21Content1),
                new Question(testData.Question22Id, testData.QuestionRepository2Id, QuestionType.SingleSelect, testData.Question22Content2),
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
                new QuestionAnswer(testData.Answer211Id, testData.Question11Id, testData.Answer211Content, false),
                new QuestionAnswer(testData.Answer212Id, testData.Question11Id, testData.Answer212Content, true),
                new QuestionAnswer(testData.Answer213Id, testData.Question11Id, testData.Answer213Content, false),
                new QuestionAnswer(testData.Answer214Id, testData.Question11Id, testData.Answer214Content, false),
                new QuestionAnswer(testData.Answer221Id, testData.Question12Id, testData.Answer221Content, false),
                new QuestionAnswer(testData.Answer222Id, testData.Question12Id, testData.Answer222Content, true),
                new QuestionAnswer(testData.Answer223Id, testData.Question12Id, testData.Answer223Content, false),
                new QuestionAnswer(testData.Answer224Id, testData.Question12Id, testData.Answer224Content, false)]);
        }
    }
}