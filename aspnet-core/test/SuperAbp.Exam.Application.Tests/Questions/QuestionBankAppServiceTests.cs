using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionBankAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionBankAppService _questionRepoAppService;

    protected QuestionBankAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionRepoAppService = GetRequiredService<IQuestionBankAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionBankListDto> result = await _questionRepoAppService.GetListAsync(new GetQuestionBanksInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        QuestionBankDetailDto result = await _questionRepoAppService.GetAsync(_testData.QuestionBank1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_QuestionTypes()
    {
        ListResultDto<QuestionType> result = await _questionRepoAppService.GetQuestionTypesAsync(_testData.QuestionBank1Id);
        result.Items.Count.ShouldBeGreaterThan(0);
    }
}