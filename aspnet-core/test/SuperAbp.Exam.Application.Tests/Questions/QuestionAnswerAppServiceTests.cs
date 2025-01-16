using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAnswerAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAnswerAppService _questionAnswerAppService;

    protected QuestionAnswerAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAnswerAppService = GetRequiredService<IQuestionAnswerAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        ListResultDto<QuestionAnswerListDto> result = await _questionAnswerAppService.GetListAsync(_testData.Question11Id);
        result.Items.Count.ShouldBeGreaterThan(0);
    }
}