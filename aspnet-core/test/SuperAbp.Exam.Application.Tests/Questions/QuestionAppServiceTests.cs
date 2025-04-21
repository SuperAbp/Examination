using System;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Validation;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAppService _questionAppService;

    protected QuestionAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAppService = GetRequiredService<IQuestionAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionListDto> result = await _questionAppService.GetListAsync(new GetQuestionsInput() { QuestionBankId = _testData.QuestionBank1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_Throw_Not_Validation()
    {
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _questionAppService.GetListAsync(new GetQuestionsInput()));
    }

    [Fact]
    public async Task Should_Get_Ids()
    {
        ListResultDto<Guid> result = await _questionAppService.GetIdsAsync(new GetQuestionsInput() { QuestionBankId = _testData.QuestionBank1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_Ids_Throw_Not_Validation()
    {
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _questionAppService.GetIdsAsync(new GetQuestionsInput()));
    }

    [Fact]
    public async Task Should_Get()
    {
        QuestionDetailDto result = await _questionAppService.GetAsync(_testData.Question11Id);
        result.ShouldNotBeNull();
    }
}