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
            await questionRepoRepository.InsertManyAsync([new(testData.QuestionRepository1Id, testData.QuestionRepository1Title), new(testData.QuestionRepository2Id, testData.QuestionRepository2Title)]);

            Question question = new(testData.Question1Id, testData.QuestionRepository1Id, QuestionType.SingleSelect,
                testData.Question1Content);
            await questionRepository.InsertAsync(question);

            await questionAnswerRepository.InsertManyAsync([
                new QuestionAnswer(testData.Answer1Id, testData.Question1Id, "测试的选项1", false),
                new QuestionAnswer(testData.Answer2Id, testData.Question1Id, "测试的选项2", true),
                new QuestionAnswer(testData.Answer3Id, testData.Question1Id, "测试的选项3", false),
                new QuestionAnswer(testData.Answer4Id, testData.Question1Id, "测试的选项4", false)]);
        }
    }
}