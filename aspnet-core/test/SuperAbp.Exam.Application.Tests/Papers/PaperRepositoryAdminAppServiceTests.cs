using SuperAbp.Exam.Admin.PaperManagement.PaperRepos;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Domain.Entities;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Validation;

namespace SuperAbp.Exam.Papers;

public abstract class PaperRepositoryAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperRepoAdminAppService _paperRepoAdminAppService;
    private readonly IPaperRepoRepository _paperRepoRepository;

    protected PaperRepositoryAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperRepoAdminAppService = GetRequiredService<IPaperRepoAdminAppService>();
        _paperRepoRepository = GetRequiredService<IPaperRepoRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<PaperRepoListDto> result = await _paperRepoAdminAppService.GetListAsync(new GetPaperReposInput() { PaperId = _testData.Paper1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_Throw_Not_Validation()
    {
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperRepoAdminAppService.GetListAsync(new GetPaperReposInput()));
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetPaperRepoForEditorOutput result = await _paperRepoAdminAppService.GetEditorAsync(_testData.PaperRepository1Id);
        result.ShouldNotBeNull();
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