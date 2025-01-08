using Shouldly;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Exams;

public abstract class UserExamAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IUserExamAppService _userExamAppService;

    protected UserExamAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamAppService = GetRequiredService<IUserExamAppService>();
    }

    // TODO: SQLite cannot apply aggregate operator 'Max' on expressions of type 'decimal'
    // [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<UserExamListDto> result = await _userExamAppService.GetListAsync(new GetUserExamsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        UserExamDetailDto result = await _userExamAppService.GetAsync(_testData.UserExam11Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get_Unfinished()
    {
        UserExamCreateDto dto = new()
        {
            ExamId = _testData.Examination11Id
        };
        await _userExamAppService.CreateAsync(dto);
        Guid? result = await _userExamAppService.GetUnfinishedAsync();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        UserExamCreateDto dto = new()
        {
            ExamId = _testData.Examination11Id
        };
        UserExamListDto repoDto = await _userExamAppService.CreateAsync(dto);

        UserExamDetailDto question = await _userExamAppService.GetAsync(repoDto.Id);
        question.ShouldNotBeNull();
        question.ExamId.ShouldBe(dto.ExamId);
    }
}