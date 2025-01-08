using SuperAbp.Exam.Admin.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.Exams;

public abstract class UserExamQuestionAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IUserExamQuestionAppService _userExamQuestionAppService;

    protected UserExamQuestionAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamQuestionAppService = GetRequiredService<IUserExamQuestionAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<UserExamQuestionListDto> result = await _userExamQuestionAppService.GetListAsync(new GetUserExamQuestionsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetUserExamQuestionForEditorOutput result = await _userExamQuestionAppService.GetEditorAsync(_testData.UserExamQuestion11Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_Detail()
    {
        UserExamQuestionDetailDto result = await _userExamQuestionAppService.GetAsync(_testData.UserExamQuestion11Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        var input = new UserExamQuestionCreateDto
        {
            QuestionId = _testData.Question11Id,
            UserExamId = _testData.UserExam11Id,
            Answers = "Sample Answer"
        };

        UserExamQuestionListDto result = await _userExamQuestionAppService.CreateAsync(input);
        GetUserExamQuestionForEditorOutput examination = await _userExamQuestionAppService.GetEditorAsync(result.Id);
        examination.ShouldNotBeNull();
        examination.QuestionId.ShouldBe(input.QuestionId);
        examination.UserExamId.ShouldBe(input.UserExamId);
        examination.Answers.ShouldBe(input.Answers);
    }

    [Fact]
    public async Task Should_Answer()
    {
        var input = new UserExamQuestionAnswerDto
        {
            Answers = "Updated Answer"
        };

        await _userExamQuestionAppService.AnswerAsync(_testData.UserExamQuestion11Id, input);
        GetUserExamQuestionForEditorOutput examination = await _userExamQuestionAppService.GetEditorAsync(_testData.UserExamQuestion11Id);
        examination.ShouldNotBeNull();
        examination.Answers.ShouldBe(input.Answers);
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _userExamQuestionAppService.DeleteAsync(_testData.Examination11Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _userExamQuestionAppService.GetEditorAsync(_testData.Examination11Id));
    }
}