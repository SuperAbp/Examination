using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Announcements;

[Authorize(ExamPermissions.Announcements.Default)]
public class AnnouncementAdminAppService(
    IAnnouncementRepository repository,
    IAnnouncementCategoryRepository categoryRepository) : ApplicationService, IAnnouncementAdminAppService
{
    protected IAnnouncementRepository Repository { get; } = repository;
    protected IAnnouncementCategoryRepository CategoryRepository { get; } = categoryRepository;

    public virtual async Task<AnnouncementDetailDto> GetAsync(Guid id)
    {
        var announcement = await Repository.GetAsync(id);
        return ObjectMapper.Map<Announcement, AnnouncementDetailDto>(announcement);
    }

    public virtual async Task<PagedResultDto<AnnouncementListDto>> GetListAsync(GetAnnouncementsInput input)
    {
        var totalCount = await Repository.GetCountAsync(
            input.Title,
            input.CategoryId,
            input.IsPublished
        );

        var items = await Repository.GetListAsync(
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            input.Title,
            input.CategoryId,
            input.IsPublished
        );

        return new PagedResultDto<AnnouncementListDto>(
            totalCount,
            ObjectMapper.Map<List<Announcement>, List<AnnouncementListDto>>(items)
        );
    }

    [Authorize(ExamPermissions.Announcements.Create)]
    public virtual async Task<AnnouncementDetailDto> CreateAsync(AnnouncementCreateDto input)
    {
        var announcement = new Announcement(
            GuidGenerator.Create(),
            input.Title,
            input.Content,
            input.Sort,
            input.CategoryId
        );

        announcement.ScheduledExpirationTime = input.ScheduledExpirationTime;

        if (input.ScheduledPublishTime.HasValue)
        {
            announcement.SetPublishTime(input.ScheduledPublishTime.Value);
        }
        else
        {
            if (input.Publish)
            {
                announcement.Publish();
            }
        }

        await Repository.InsertAsync(announcement);
        return ObjectMapper.Map<Announcement, AnnouncementDetailDto>(announcement);
    }

    [Authorize(ExamPermissions.Announcements.Update)]
    public virtual async Task<AnnouncementDetailDto> UpdateAsync(Guid id, AnnouncementUpdateDto input)
    {
        var announcement = await Repository.GetAsync(id);

        if (announcement.IsPublished)
        {
            throw new AnnouncementAlreadyPublishedException();
        }

        announcement.Title = input.Title;
        announcement.Content = input.Content;
        announcement.Sort = input.Sort;
        announcement.CategoryId = input.CategoryId;
        announcement.ScheduledExpirationTime = input.ScheduledExpirationTime;

        if (input.ScheduledPublishTime.HasValue)
        {
            announcement.SetPublishTime(input.ScheduledPublishTime.Value);
        }
        else
        {
            if (input.Publish)
            {
                announcement.Publish();
            }
            announcement.ScheduledPublishTime = null;
        }

        await Repository.UpdateAsync(announcement);
        return ObjectMapper.Map<Announcement, AnnouncementDetailDto>(announcement);
    }

    [Authorize(ExamPermissions.Announcements.Publish)]
    public virtual async Task PublishAsync(Guid id)
    {
        var announcement = await Repository.GetAsync(id);
        announcement.Publish();
        await Repository.UpdateAsync(announcement);
    }

    [Authorize(ExamPermissions.Announcements.Unpublish)]
    public virtual async Task UnpublishAsync(Guid id)
    {
        var announcement = await Repository.GetAsync(id);
        announcement.Unpublish();
        await Repository.UpdateAsync(announcement);
    }

    [Authorize(ExamPermissions.Announcements.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await Repository.DeleteAsync(id);
    }
}