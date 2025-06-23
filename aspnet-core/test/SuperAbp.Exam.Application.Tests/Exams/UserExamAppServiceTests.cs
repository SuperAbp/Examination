using Shouldly;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Xunit;

namespace SuperAbp.Exam.Exams;

public abstract class UserExamAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IUserExamAppService _userExamAppService;
    private readonly IUserExamRepository _userExamRepository;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    protected UserExamAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamAppService = GetRequiredService<IUserExamAppService>();
        _userExamRepository = GetRequiredService<IUserExamRepository>();
        _currentPrincipalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
    }

    [Fact]
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
        Guid? result = await _userExamAppService.GetUnfinishedAsync();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        UserExamCreateDto input = new()
        {
            ExamId = _testData.Examination12Id
        };
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User2Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            UserExamListDto repoDto = await _userExamAppService.CreateAsync(input);
            UserExam userExam = await _userExamRepository.GetAsync(repoDto.Id);
            userExam.ShouldNotBeNull();
            userExam.Status.ShouldBe(UserExamStatus.InProgress);
            userExam.ExamId.ShouldBe(input.ExamId);
        }
    }

    [Fact]
    public async Task Should_Create_Throw_OutOfExamTimeException()
    {
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User2Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            UserExamCreateDto input = new()
            {
                ExamId = _testData.Examination31Id
            };
            await Should.ThrowAsync<OutOfExamTimeException>(async () =>
                await _userExamAppService.CreateAsync(input));
        }
    }

    [Fact]
    public async Task Should_Create_Throw_UnfinishedAlreadyExistException()
    {
        UserExamCreateDto input = new()
        {
            ExamId = _testData.Examination12Id
        };
        await Should.ThrowAsync<UnfinishedAlreadyExistException>(async () =>
        {
            await _userExamAppService.CreateAsync(input);
        });
    }

    [Fact]
    public async Task Should_Create_Throw_InvalidExamStatusException()
    {
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User2Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            UserExamCreateDto input = new()
            {
                ExamId = _testData.Examination11Id
            };
            await Should.ThrowAsync<InvalidExamStatusException>(async () =>
                await _userExamAppService.CreateAsync(input));
        }
    }

    [Fact]
    public async Task Should_Answer()
    {
        await _userExamAppService.AnswerAsync(_testData.UserExam11Id,
            new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" });
    }

    [Fact]
    public async Task Should_Answer_Throw_InvalidExamStatusException()
    {
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User3Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            await Should.ThrowAsync<InvalidExamStatusException>(async () =>
                await _userExamAppService.AnswerAsync(_testData.UserExam21Id,
                    new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }));
        }
    }

    [Fact]
    public async Task Should_Answer_Throw_InvalidUserExamStatusException()
    {
        await Should.ThrowAsync<InvalidUserExamStatusException>(async () =>
            await _userExamAppService.AnswerAsync(_testData.UserExam12Id,
                new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }));
    }

    [Fact]
    public async Task Should_Finished()
    {
        await _userExamAppService.FinishedAsync(_testData.UserExam11Id,
            [new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }]);
    }

    [Fact]
    public async Task Should_Finished_Throw_InvalidExamStatusException()
    {
        using (_currentPrincipalAccessor.Change([
                   new Claim(AbpClaimTypes.UserId, _testData.User3Id.ToString()),
                   new Claim(AbpClaimTypes.UserName, "test1"),
                   new Claim(AbpClaimTypes.Email, "test1@abp.io")
               ]))
        {
            await Should.ThrowAsync<InvalidExamStatusException>(async () =>
                await _userExamAppService.FinishedAsync(_testData.UserExam21Id,
                    [new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }]));
        }
    }

    [Fact]
    public async Task Should_Finished_Throw_InvalidUserExamStatusException()
    {
        await Should.ThrowAsync<InvalidUserExamStatusException>(async () =>
            await _userExamAppService.FinishedAsync(_testData.UserExam12Id,
                [new UserExamAnswerDto() { QuestionId = _testData.Question11Id, Answers = "A" }]));
    }
}