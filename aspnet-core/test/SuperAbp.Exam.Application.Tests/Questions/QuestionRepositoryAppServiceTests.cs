using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionRepositoryAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionRepoAppService _questionRepoAppService;

    protected QuestionRepositoryAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionRepoAppService = GetRequiredService<IQuestionRepoAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionRepoListDto> result = await _questionRepoAppService.GetListAsync(new GetQuestionReposInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        QuestionRepoDetailDto result = await _questionRepoAppService.GetAsync(_testData.QuestionRepository1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_QuestionTypes()
    {
        ListResultDto<QuestionType> result = await _questionRepoAppService.GetQuestionTypesAsync(_testData.QuestionRepository1Id);
        result.Items.Count.ShouldBeGreaterThan(0);
    }
}