using Shouldly;
using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Announcements;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Admin.Announcements;

public abstract class AnnouncementCategoryAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAnnouncementCategoryAdminAppService _adminAppService;
    private readonly ExamTestData _testData;

    protected AnnouncementCategoryAdminAppServiceTests()
    {
        _adminAppService = GetRequiredService<IAnnouncementCategoryAdminAppService>();
        _testData = GetRequiredService<ExamTestData>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _adminAppService.GetListAsync();
        result.Items.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Should_Get()
    {
        var result = await _adminAppService.GetAsync(_testData.AnnouncementCategory1Id);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(_testData.AnnouncementCategory1Name);
    }

    [Fact]
    public async Task Should_Create()
    {
        var input = new AnnouncementCategoryCreateDto
        {
            Name = "New Category",
            Sort = 1,
            Remark = "Test Remark"
        };

        var result = await _adminAppService.CreateAsync(input);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(input.Name);
        result.Sort.ShouldBe(input.Sort);
        result.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Update()
    {
        var input = new AnnouncementCategoryUpdateDto
        {
            Name = "Updated Category",
            Sort = 10,
            Remark = "Updated Remark"
        };

        var result = await _adminAppService.UpdateAsync(_testData.AnnouncementCategory1Id, input);
        result.ShouldNotBeNull();
        result.Name.ShouldBe(input.Name);
        result.Sort.ShouldBe(input.Sort);
        result.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _adminAppService.DeleteAsync(_testData.AnnouncementCategory1Id);

        await Should.ThrowAsync<EntityNotFoundException>(async () => await _adminAppService.GetAsync(_testData.AnnouncementCategory1Id));
    }
}