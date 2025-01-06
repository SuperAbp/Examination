using Shouldly;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
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

    protected PaperAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperAdminAppService = GetRequiredService<IPaperAdminAppService>();
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
        PaperCreateDto dto = new()
        {
            Name = "New_Name",
            Description = "New_Description",
            Score = 100
        };
        PaperListDto questionDto = await _paperAdminAppService.CreateAsync(dto);
        GetPaperForEditorOutput result = await _paperAdminAppService.GetEditorAsync(questionDto.Id);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(dto.Name);
        result.Description.ShouldBe(dto.Description);
        result.Score.ShouldBe(dto.Score);
    }

    [Fact]
    public async Task Should_Update()
    {
        PaperUpdateDto dto = new()
        {
            Name = "Update_Name",
            Description = "Update_Description",
            Score = int.MaxValue
        };
        await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, dto);
        GetPaperForEditorOutput question = await _paperAdminAppService.GetEditorAsync(_testData.Paper1Id);
        question.ShouldNotBeNull();
        question.Name.ShouldBe(dto.Name);
        question.Description.ShouldBe(dto.Description);
        question.Score.ShouldBe(dto.Score);
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