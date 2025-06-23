using System.Collections.Generic;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.ExamManagement.UserExams;
using Volo.Abp.Modularity;
using Xunit;
using System.Security.Claims;
using Volo.Abp.Security.Claims;

namespace SuperAbp.Exam.Exams;

public abstract class UserExamAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IUserExamAdminAppService _userExamAppService;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public UserExamAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamAppService = GetRequiredService<IUserExamAdminAppService>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _userExamAppService.GetListAsync(new GetUserExamsInput() { ExamId = _testData.Examination12Id, UserId = _testData.User1Id });
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_With_User()
    {
        var result = await _userExamAppService.GetListWithUserAsync(new GetUserExamWithUsersInput() { ExamId = _testData.Examination12Id });
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Review_Questions()
    {
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User3Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            await _userExamAppService.ReviewQuestionsAsync(_testData.UserExam22Id,
                [new ReviewedQuestionDto() { QuestionId = _testData.Question11Id, Right = true }]);
        }
    }
}