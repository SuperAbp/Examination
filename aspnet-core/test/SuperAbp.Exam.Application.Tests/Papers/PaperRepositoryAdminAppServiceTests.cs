using SuperAbp.Exam.Admin.PaperManagement.PaperRepos;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.Papers;

public abstract class PaperRepositoryAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperRepoAdminAppService _paperRepoAdminAppService;

    protected PaperRepositoryAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperRepoAdminAppService = GetRequiredService<IPaperRepoAdminAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<PaperRepoListDto> result = await _paperRepoAdminAppService.GetListAsync(new GetPaperReposInput() { PaperId = _testData.Paper1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetPaperRepoForEditorOutput result = await _paperRepoAdminAppService.GetEditorAsync(_testData.PaperRepository1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        PaperRepoCreateDto dto = new()
        {
            PaperId = _testData.Paper1Id,
            QuestionRepositoryId = _testData.QuestionRepository1Id,
            SingleCount = 1,
            SingleScore = 1,
            MultiCount = 1,
            MultiScore = 1,
            JudgeCount = 1,
            JudgeScore = 1,
            BlankCount = 1,
            BlankScore = 1
        };
        PaperRepoListDto questionDto = await _paperRepoAdminAppService.CreateAsync(dto);
        GetPaperRepoForEditorOutput result = await _paperRepoAdminAppService.GetEditorAsync(questionDto.Id);
        result.ShouldNotBeNull();
        result.SingleCount.ShouldBe(dto.SingleCount);
        result.SingleScore.ShouldBe(dto.SingleScore);
        result.MultiCount.ShouldBe(dto.MultiCount);
        result.MultiScore.ShouldBe(dto.MultiScore);
        result.JudgeCount.ShouldBe(dto.JudgeCount);
        result.JudgeScore.ShouldBe(dto.JudgeScore);
        result.BlankCount.ShouldBe(dto.BlankCount);
        result.BlankScore.ShouldBe(dto.BlankScore);
    }

    [Fact]
    public async Task Should_Update()
    {
        PaperRepoUpdateDto dto = new()
        {
            SingleCount = int.MaxValue,
            SingleScore = decimal.MaxValue,
            MultiCount = int.MaxValue,
            MultiScore = decimal.MaxValue,
            JudgeCount = int.MaxValue,
            JudgeScore = decimal.MaxValue,
            BlankCount = int.MaxValue,
            BlankScore = decimal.MaxValue
        };
        await _paperRepoAdminAppService.UpdateAsync(_testData.PaperRepository1Id, dto);
        GetPaperRepoForEditorOutput result = await _paperRepoAdminAppService.GetEditorAsync(_testData.PaperRepository1Id);
        result.ShouldNotBeNull();
        result.SingleCount.ShouldBe(dto.SingleCount);
        result.SingleScore.ShouldBe(dto.SingleScore);
        result.MultiCount.ShouldBe(dto.MultiCount);
        result.MultiScore.ShouldBe(dto.MultiScore);
        result.JudgeCount.ShouldBe(dto.JudgeCount);
        result.JudgeScore.ShouldBe(dto.JudgeScore);
        result.BlankCount.ShouldBe(dto.BlankCount);
        result.BlankScore.ShouldBe(dto.BlankScore);
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _paperRepoAdminAppService.DeleteAsync(_testData.PaperRepository1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _paperRepoAdminAppService.GetEditorAsync(_testData.PaperRepository1Id));
    }
}