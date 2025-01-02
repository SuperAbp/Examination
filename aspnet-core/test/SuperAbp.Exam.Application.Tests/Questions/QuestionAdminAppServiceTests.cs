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
using SuperAbp.Exam.QuestionManagement.QuestionRepos;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionAdminAppService _questionAppService;

    protected QuestionAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionAppService = GetRequiredService<IQuestionAdminAppService>();
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
        GetQuestionForEditorOutput result = await _questionAppService.GetEditorAsync(_testData.Question1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionCreateDto dto = new()
        {
            QuestionRepositoryId = _testData.QuestionRepository1Id,
            QuestionType = QuestionType.MultiSelect,
            Content = "New_Content",
            Analysis = "New_Analysis"
        };
        QuestionListDto questionDto = await _questionAppService.CreateAsync(dto);
        GetQuestionForEditorOutput question = await _questionAppService.GetEditorAsync(questionDto.Id);
        question.ShouldNotBeNull();
        question.Content.ShouldBe(dto.Content);
        question.QuestionRepositoryId.ShouldBe(dto.QuestionRepositoryId);
        question.Analysis.ShouldBe(dto.Analysis);
    }

    [Fact]
    public async Task Should_Create_Throw_Exists_Content()
    {
        QuestionCreateDto dto = new()
        {
            QuestionRepositoryId = _testData.QuestionRepository1Id,
            QuestionType = QuestionType.MultiSelect,
            Content = _testData.Question1Content1,
            Analysis = "New_Analysis"
        };
        await Should.ThrowAsync<QuestionContentAlreadyExistException>(
            async () => await _questionAppService.CreateAsync(dto));
    }

    [Fact]
    public async Task Should_Update()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            QuestionUpdateDto dto = new()
            {
                QuestionRepositoryId = _testData.QuestionRepository2Id,
                Content = "Update_Content",
                Analysis = "Update_Analysis"
            };
            await _questionAppService.UpdateAsync(_testData.Question1Id, dto);
            GetQuestionForEditorOutput question = await _questionAppService.GetEditorAsync(_testData.Question1Id);
            question.ShouldNotBeNull();
            question.QuestionRepositoryId.ShouldBe(dto.QuestionRepositoryId);
            question.Content.ShouldBe(dto.Content);
            question.Analysis.ShouldBe(dto.Analysis);
        });
    }

    [Fact]
    public async Task Should_Update_Throw_Exists_Content()
    {
        QuestionUpdateDto dto = new()
        {
            QuestionRepositoryId = _testData.QuestionRepository1Id,
            Content = _testData.Question1Content2,
            Analysis = "Update_Analysis"
        };
        await Should.ThrowAsync<QuestionContentAlreadyExistException>(
            async () => await _questionAppService.UpdateAsync(_testData.Question1Id, dto));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionAppService.DeleteAsync(_testData.Question1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionAppService.GetEditorAsync(_testData.Question1Id));
    }
}