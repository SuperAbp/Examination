using System;
using Shouldly;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAnswerAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAnswerAdminAppService _questionAnswerAppService;

    protected QuestionAnswerAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAnswerAppService = GetRequiredService<IQuestionAnswerAdminAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionAnswerListDto> result = await _questionAnswerAppService.GetListAsync(new GetQuestionAnswersInput { QuestionId = _testData.Question11Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetQuestionAnswerForEditorOutput result = await _questionAnswerAppService.GetEditorAsync(_testData.Answer111Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionAnswerCreateDto dto = new()
        {
            QuestionId = _testData.Question11Id,
            Content = "New_Content",
            Analysis = "New_Analysis",
            Sort = 1
        };
        QuestionAnswerListDto questionDto = await _questionAnswerAppService.CreateAsync(dto);

        GetQuestionAnswerForEditorOutput question = await _questionAnswerAppService.GetEditorAsync(questionDto.Id);
        question.ShouldNotBeNull();
        question.Content.ShouldBe(dto.Content);
        question.Analysis.ShouldBe(dto.Analysis);
    }

    [Fact]
    public async Task Should_Create_Throw_Exist_Content()
    {
        QuestionAnswerCreateDto dto = new()
        {
            QuestionId = _testData.Question11Id,
            Content = _testData.Answer111Content,
            Analysis = "New_Analysis",
            Sort = 1
        };
        await Should.ThrowAsync<QuestionAnswerContentAlreadyExistException>(
            async () => await _questionAnswerAppService.CreateAsync(dto));
    }

    [Fact]
    public async Task Should_Update()
    {
        QuestionAnswerUpdateDto dto = new()
        {
            Content = "Update_Content",
            Analysis = "Update_Analysis",
            Sort = Int32.MaxValue - 1,
            Right = false
        };
        await _questionAnswerAppService.UpdateAsync(_testData.Answer111Id, dto);
        GetQuestionAnswerForEditorOutput question = await _questionAnswerAppService.GetEditorAsync(_testData.Answer111Id);
        question.ShouldNotBeNull();
        question.Content.ShouldBe(dto.Content);
        question.Analysis.ShouldBe(dto.Analysis);
        question.Sort.ShouldBe(dto.Sort);
        question.Right.ShouldBe(dto.Right);
    }

    [Fact]
    public async Task Should_Update_Throw_Exist_Content()
    {
        QuestionAnswerUpdateDto dto = new()
        {
            Content = _testData.Answer112Content,
            Analysis = "Update_Analysis",
            Sort = Int32.MaxValue - 1,
            Right = false
        };
        await Should.ThrowAsync<QuestionAnswerContentAlreadyExistException>(
            async () => await _questionAnswerAppService.UpdateAsync(_testData.Answer111Id, dto));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionAnswerAppService.DeleteAsync(_testData.Answer111Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionAnswerAppService.GetEditorAsync(_testData.Answer111Id));
    }
}