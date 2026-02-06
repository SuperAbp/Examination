using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Announcements;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

[Route("api/announcements")]
public class AnnouncementController(IAnnouncementAdminAppService announcementAppService) : AbpController, IAnnouncementAdminAppService
{
    protected IAnnouncementAdminAppService AnnouncementAppService { get; } = announcementAppService;

    [HttpGet("{id}")]
    public async Task<AnnouncementDetailDto> GetAsync(Guid id)
    {
        return await AnnouncementAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual async Task<PagedResultDto<AnnouncementListDto>> GetListAsync(GetAnnouncementsInput input)
    {
        return await AnnouncementAppService.GetListAsync(input);
    }

    [HttpPost]
    public virtual async Task<AnnouncementDetailDto> CreateAsync(AnnouncementCreateDto input)
    {
        return await AnnouncementAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual async Task<AnnouncementDetailDto> UpdateAsync(Guid id, AnnouncementUpdateDto input)
    {
        return await AnnouncementAppService.UpdateAsync(id, input);
    }

    [HttpPatch("{id}/publish")]
    public virtual async Task PublishAsync(Guid id)
    {
        await AnnouncementAppService.PublishAsync(id);
    }

    [HttpPatch("{id}/unpublish")]
    public virtual async Task UnpublishAsync(Guid id)
    {
        await AnnouncementAppService.UnpublishAsync(id);
    }

    [HttpDelete("{id}")]
    public virtual async Task DeleteAsync(Guid id)
    {
        await AnnouncementAppService.DeleteAsync(id);
    }
}