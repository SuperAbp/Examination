using Shouldly;
using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Announcements;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Admin.Announcements;

public abstract class AnnouncementAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAnnouncementRepository _repository;
    private readonly IAnnouncementAdminAppService _adminAppService;
    private readonly ExamTestData _testData;

    protected AnnouncementAdminAppServiceTests()
    {
        _repository = GetRequiredService<IAnnouncementRepository>();
        _adminAppService = GetRequiredService<IAnnouncementAdminAppService>();
        _testData = GetRequiredService<ExamTestData>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _adminAppService.GetListAsync(new GetAnnouncementsInput { MaxResultCount = 10 });
        result.TotalCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        var result = await _adminAppService.GetAsync(_testData.Announcement1Id);
        result.ShouldNotBeNull();
        result.Title.ShouldBe(_testData.Announcement1Title);
    }

    [Fact]
    public async Task Should_Create()
    {
        var input = new AnnouncementCreateDto
        {
            Title = "New Announcement",
            Content = "New Content",
            Sort = 1,
            CategoryId = _testData.AnnouncementCategory1Id
        };

        var result = await _adminAppService.CreateAsync(input);
        result.ShouldNotBeNull();
        result.Title.ShouldBe(input.Title);
        result.Content.ShouldBe(input.Content);
        result.IsPublished.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_Update()
    {
        var input = new AnnouncementUpdateDto
        {
            Title = "Updated Announcement",
            Content = "Updated Content",
            Sort = 10,
            CategoryId = _testData.AnnouncementCategory1Id
        };

        var result = await _adminAppService.UpdateAsync(_testData.Announcement1Id, input);
        result.ShouldNotBeNull();
        result.Title.ShouldBe(input.Title);
        result.Content.ShouldBe(input.Content);
        result.Sort.ShouldBe(input.Sort);
    }

    [Fact]
    public async Task Should_Publish()
    {
        await _adminAppService.PublishAsync(_testData.Announcement4Id);

        var updatedAnnouncement = await _repository.GetAsync(_testData.Announcement4Id);
        updatedAnnouncement.IsPublished.ShouldBeTrue();
        updatedAnnouncement.PublishTime.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Unpublish()
    {
        await _adminAppService.UnpublishAsync(_testData.Announcement1Id);

        var updatedAnnouncement = await _repository.GetAsync(_testData.Announcement1Id);
        updatedAnnouncement.IsPublished.ShouldBeFalse();
        updatedAnnouncement.PublishTime.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _adminAppService.DeleteAsync(_testData.Announcement1Id);

        await Should.ThrowAsync<EntityNotFoundException>(async () => await _repository.GetAsync(_testData.Announcement1Id));
    }
}