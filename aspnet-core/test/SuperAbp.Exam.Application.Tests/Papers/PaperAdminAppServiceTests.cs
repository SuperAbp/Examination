using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Papers;

public abstract class PaperAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperAdminAppService _paperAdminAppService;
    private readonly IPaperRepository _paperRepository;

    protected PaperAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperAdminAppService = GetRequiredService<IPaperAdminAppService>();
        _paperRepository = GetRequiredService<IPaperRepository>();
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
        PaperCreateDto input = new()
        {
            Name = "New_Name",
            Description = "New_Description",
            Score = 100
        };
        PaperListDto dto = await _paperAdminAppService.CreateAsync(input);
        Paper paper = await _paperRepository.GetAsync(dto.Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
    }

    [Fact]
    public async Task Should_Create_Throw_Exists_Content()
    {
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 100
        };
        await Should.ThrowAsync<PaperNameAlreadyExistException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Update()
    {
        PaperUpdateDto input = new()
        {
            Name = "Update_Name",
            Description = "Update_Description",
            Score = int.MaxValue
        };
        await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input);
        Paper paper = await _paperRepository.GetAsync(_testData.Paper1Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
    }

    [Fact]
    public async Task Should_Update_Throw_Exists_Content()
    {
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper2Name,
            Description = "Update_Description",
            Score = int.MaxValue
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