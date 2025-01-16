using System;
using Shouldly;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using Volo.Abp.Validation;
using QuestionAnswerListDto = SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers.QuestionAnswerListDto;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAnswerAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAnswerAdminAppService _questionAnswerAppService;
    private readonly IQuestionAnswerRepository _questionAnswerRepository;

    protected QuestionAnswerAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAnswerAppService = GetRequiredService<IQuestionAnswerAdminAppService>();
        _questionAnswerRepository = GetRequiredService<IQuestionAnswerRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionAnswerListDto> result = await _questionAnswerAppService.GetListAsync(new GetQuestionAnswersInput { QuestionId = _testData.Question11Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_Throw_Not_Validation()
    {
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _questionAnswerAppService.GetListAsync(new GetQuestionAnswersInput()));
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
        QuestionAnswerCreateDto input = new()
        {
            QuestionId = _testData.Question11Id,
            Content = "New_Content",
            Analysis = "New_Analysis",
            Sort = 1
        };
        QuestionAnswerListDto dto = await _questionAnswerAppService.CreateAsync(input);
        QuestionAnswer questionAnswer = await _questionAnswerRepository.GetAsync(dto.Id);
        questionAnswer.ShouldNotBeNull();
        questionAnswer.Content.ShouldBe(input.Content);
        questionAnswer.Analysis.ShouldBe(input.Analysis);
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
        QuestionAnswerUpdateDto input = new()
        {
            Content = "Update_Content",
            Analysis = "Update_Analysis",
            Sort = Int32.MaxValue - 1,
            Right = false
        };
        await _questionAnswerAppService.UpdateAsync(_testData.Answer111Id, input);
        QuestionAnswer questionAnswer = await _questionAnswerRepository.GetAsync(_testData.Answer111Id);
        questionAnswer.ShouldNotBeNull();
        questionAnswer.Content.ShouldBe(input.Content);
        questionAnswer.Analysis.ShouldBe(input.Analysis);
        questionAnswer.Sort.ShouldBe(input.Sort);
        questionAnswer.Right.ShouldBe(input.Right);
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