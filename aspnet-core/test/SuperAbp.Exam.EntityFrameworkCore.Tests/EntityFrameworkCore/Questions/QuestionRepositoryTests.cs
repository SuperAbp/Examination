using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.QuestionManagement.Questions;
using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Questions;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class QuestionRepositoryTests : ExamEntityFrameworkCoreTestBase
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionRepositoryTests()
    {
        _questionRepository = GetRequiredService<IQuestionRepository>();
    }

    [Fact]
    public async Task Should_Get_Count()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            long count = await _questionRepository.GetCountAsync();

            count.ShouldBeGreaterThan(0);
        });
    }
}