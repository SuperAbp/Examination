using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using GetQuestionsInput = SuperAbp.Exam.Admin.QuestionManagement.Questions.GetQuestionsInput;
using IQuestionAdminAppService = SuperAbp.Exam.Admin.QuestionManagement.Questions.IQuestionAdminAppService;
using QuestionListDto = SuperAbp.Exam.Admin.QuestionManagement.Questions.QuestionListDto;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAdminAppService _questionAppService;
    private readonly IQuestionRepository _questionRepository;

    protected QuestionAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAppService = GetRequiredService<IQuestionAdminAppService>();
        _questionRepository = GetRequiredService<IQuestionRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionListDto> result = await _questionAppService.GetListAsync(new GetQuestionsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetQuestionForEditorOutput result = await _questionAppService.GetEditorAsync(_testData.Question11Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionCreateDto input = new()
        {
            QuestionBankId = _testData.QuestionBank1Id,
            QuestionType = QuestionType.MultiSelect.Value,
            Content = "New_Content",
            Analysis = "New_Analysis",
            Options = [new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content1" }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content2", Right = true }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content3", Right = true }]
        };
        QuestionListDto dto = await _questionAppService.CreateAsync(input);
        Question question = await _questionRepository.GetAsync(dto.Id);
        question.ShouldNotBeNull();
        question.Content.ShouldBe(input.Content);
        question.QuestionType.ShouldBe(QuestionType.FromValue(input.QuestionType));
        question.QuestionBankId.ShouldBe(input.QuestionBankId);
        question.Analysis.ShouldBe(input.Analysis);
    }

    [Fact]
    public async Task Should_Create_Throw_Correct_Count_Error()
    {
        QuestionCreateDto input = new()
        {
            QuestionBankId = _testData.QuestionBank1Id,
            QuestionType = QuestionType.MultiSelect.Value,
            Content = "New_Content",
            Analysis = "New_Analysis"
        };
        await Should.ThrowAsync<QuestionAnswerCorrectCountErrorException>(
            async () => await _questionAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Create_Throw_Exists_Content()
    {
        QuestionCreateDto input = new()
        {
            QuestionBankId = _testData.QuestionBank1Id,
            QuestionType = QuestionType.MultiSelect.Value,
            Content = _testData.Question11Content,
            Analysis = "New_Analysis",
            Options = [new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content1" }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content2", Right = true }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content3", Right = true }]
        };
        await Should.ThrowAsync<QuestionContentAlreadyExistException>(
            async () => await _questionAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Update()
    {
        QuestionUpdateDto input = new()
        {
            QuestionBankId = _testData.QuestionBank2Id,
            Content = "Update_Content",
            Analysis = "Update_Analysis",
            Options = [new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content1" }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content2", Right = true }]
        };
        await _questionAppService.UpdateAsync(_testData.Question11Id, input);
        Question question = await _questionRepository.GetAsync(_testData.Question11Id);
        question.ShouldNotBeNull();
        question.QuestionBankId.ShouldBe(input.QuestionBankId);
        question.Content.ShouldBe(input.Content);
        question.Analysis.ShouldBe(input.Analysis);
    }

    [Fact]
    public async Task Should_Update_Throw_Exists_Content()
    {
        QuestionUpdateDto input = new()
        {
            QuestionBankId = _testData.QuestionBank1Id,
            Content = _testData.Question12Content,
            Analysis = "Update_Analysis",
            Options = [new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content1" }, new QuestionCreateOrUpdateAnswerDto() { Content = "New_Content2", Right = true }]
        };
        await Should.ThrowAsync<QuestionContentAlreadyExistException>(
            async () => await _questionAppService.UpdateAsync(_testData.Question11Id, input));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionAppService.DeleteAsync(_testData.Question11Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionAppService.GetEditorAsync(_testData.Question11Id));
    }
}