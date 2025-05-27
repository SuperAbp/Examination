using Shouldly;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Collections.Generic;
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
    private readonly IUserExamRepository _userExamRepository;

    protected UserExamAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamAppService = GetRequiredService<IUserExamAppService>();
        _userExamRepository = GetRequiredService<IUserExamRepository>();
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
        UserExamCreateDto input = new()
        {
            ExamId = _testData.Examination11Id
        };
        await _userExamAppService.CreateAsync(input);
        Guid? result = await _userExamAppService.GetUnfinishedAsync();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        UserExamCreateDto input = new()
        {
            ExamId = _testData.Examination11Id
        };
        UserExamListDto repoDto = await _userExamAppService.CreateAsync(input);
        UserExam userExam = await _userExamRepository.GetAsync(repoDto.Id);
        userExam.ShouldNotBeNull();
        userExam.Status.ShouldBe(UserExamStatus.InProgress);
        userExam.ExamId.ShouldBe(input.ExamId);
    }

    [Fact]
    public async Task Should_Answer()
    {
        await _userExamAppService.AnswerAsync(_testData.UserExam11Id,
            new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" });
    }

    [Fact]
    public async Task Should_Finished()
    {
        await _userExamAppService.FinishedAsync(_testData.UserExam11Id,
            [new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }]);
    }
}