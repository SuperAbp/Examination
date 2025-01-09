using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace SuperAbp.Exam.Papers;

public abstract class PaperAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperAdminAppService _paperAdminAppService;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperRepoRepository _paperRepoRepository;

    protected PaperAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperAdminAppService = GetRequiredService<IPaperAdminAppService>();
        _paperRepository = GetRequiredService<IPaperRepository>();
        _paperRepoRepository = GetRequiredService<IPaperRepoRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<PaperListDto> result = await _paperAdminAppService.GetListAsync(new GetPapersInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetPaperForEditorOutput result = await _paperAdminAppService.GetEditorAsync(_testData.Paper1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        PaperCreateDto.PaperCreatePaperRepoDto[] repositories =
        [
            new PaperCreateDto.PaperCreatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperCreateDto input = new()
        {
            Name = "New_Name",
            Description = "New_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            Repositories = repositories
        };
        PaperListDto dto = await _paperAdminAppService.CreateAsync(input);
        Paper paper = await _paperRepository.GetAsync(dto.Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
        List<PaperRepo> paperRepos = await _paperRepoRepository.GetListAsync(paperId: paper.Id);
        paperRepos.Count.ShouldBe(input.Repositories.Length);
    }

    [Fact]
    public async Task Should_Create_Throw_Exists_Content()
    {
        PaperCreateDto.PaperCreatePaperRepoDto[] repositories =
        [
            new PaperCreateDto.PaperCreatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            Repositories = repositories
        };
        await Should.ThrowAsync<PaperNameAlreadyExistException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Create_Throw_Required_Repository()
    {
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Create_Throw_Invalid_Score()
    {
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0,
            Repositories = [
                new PaperCreateDto.PaperCreatePaperRepoDto()
                {
                    QuestionRepositoryId = _testData.QuestionRepository1Id,
                    SingleCount = 1,
                    SingleScore = 1,
                    MultiCount = 1,
                    MultiScore = 1,
                    JudgeCount = 1,
                    JudgeScore = 1,
                    BlankCount = 1,
                    BlankScore = 1
                }
            ]
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Update()
    {
        PaperUpdateDto.PaperUpdatePaperRepoDto[] repositories =
        [
            new PaperUpdateDto.PaperUpdatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
            new PaperUpdateDto.PaperUpdatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository2Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperUpdateDto input = new()
        {
            Name = "Update_Name",
            Description = "Update_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            Repositories = repositories
        };
        await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input);
        Paper paper = await _paperRepository.GetAsync(_testData.Paper1Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
        List<PaperRepo> paperRepos = await _paperRepoRepository.GetListAsync(paperId: paper.Id);
        paperRepos.Count.ShouldBe(input.Repositories.Length);
    }

    [Fact]
    public async Task Should_Update_Throw_Required_Repository()
    {
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Update_Throw_Invalid_Score()
    {
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0,
            Repositories = [
                new PaperUpdateDto.PaperUpdatePaperRepoDto()
                {
                    QuestionRepositoryId = _testData.QuestionRepository1Id,
                    SingleCount = 1,
                    SingleScore = 1,
                    MultiCount = 1,
                    MultiScore = 1,
                    JudgeCount = 1,
                    JudgeScore = 1,
                    BlankCount = 1,
                    BlankScore = 1
                }
            ]
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Update_Throw_Exists_Content()
    {
        PaperUpdateDto.PaperUpdatePaperRepoDto[] repositories =
        [
            new PaperUpdateDto.PaperUpdatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
            new PaperUpdateDto.PaperUpdatePaperRepoDto()
            {
                QuestionRepositoryId = _testData.QuestionRepository2Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper2Name,
            Description = "Update_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            Repositories = repositories
        };
        await Should.ThrowAsync<PaperNameAlreadyExistException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _paperAdminAppService.DeleteAsync(_testData.Paper1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _paperAdminAppService.GetEditorAsync(_testData.Paper1Id));
    }
}