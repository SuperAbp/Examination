using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.Announcements;

namespace SuperAbp.Exam.Announcements;

public abstract class AnnouncementAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IAnnouncementAppService _appService;
    private readonly ExamTestData _testData;

    protected AnnouncementAppServiceTests()
    {
        _appService = GetRequiredService<IAnnouncementAppService>();
        _testData = GetRequiredService<ExamTestData>();
    }

    [Fact]
    public async Task Should_Get_Effective_List()
    {
        var result = await _appService.GetEffectiveListAsync(_testData.AnnouncementCategory1Id);
        result.Items.ShouldNotBeNull();
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        var result = await _appService.GetAsync(_testData.Announcement1Id);
        result.ShouldNotBeNull();
        result.Title.ShouldBe(_testData.Announcement1Title);
        result.CategoryName.ShouldBe(_testData.AnnouncementCategory1Name);
    }

    [Fact]
    public async Task Should_Throw_When_Get_Not_Effective_Announcement()
    {
        await Should.ThrowAsync<Volo.Abp.BusinessException>(async () => await _appService.GetAsync(_testData.Announcement4Id));
    }
}