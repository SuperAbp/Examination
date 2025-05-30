using System.Collections.Generic;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.ExamManagement.UserExams;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Exams;

public abstract class UserExamAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IUserExamAdminAppService _userExamAppService;

    public UserExamAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamAppService = GetRequiredService<IUserExamAdminAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _userExamAppService.GetListAsync(new GetUserExamsInput() { ExamId = _testData.Examination11Id, UserId = _testData.User1Id });
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_With_User()
    {
        var result = await _userExamAppService.GetListWithUserAsync(new GetUserExamWithUsersInput() { ExamId = _testData.Examination11Id });
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Review_Questions()
    {
        await _userExamAppService.ReviewQuestionsAsync(_testData.UserExam13Id, [new ReviewedQuestionDto() { QuestionId = _testData.Question11Id, Right = true }]);
    }
}