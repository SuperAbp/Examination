using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Announcements;

public abstract class AnnouncementCategoryAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAnnouncementCategoryAppService _appService;
    private readonly ExamTestData _testData;

    protected AnnouncementCategoryAppServiceTests()
    {
        _appService = GetRequiredService<IAnnouncementCategoryAppService>();
        _testData = GetRequiredService<ExamTestData>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _appService.GetListAsync();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        var result = await _appService.GetAsync(_testData.AnnouncementCategory1Id);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(_testData.AnnouncementCategory1Name);
    }
}