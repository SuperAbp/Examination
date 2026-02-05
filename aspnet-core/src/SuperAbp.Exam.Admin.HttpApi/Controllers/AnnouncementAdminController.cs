using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Announcements;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

[Route("api/announcements")]
public class AnnouncementAdminController : AbpController, IAnnouncementAdminAppService
{
    private readonly IAnnouncementAdminAppService _announcementAppService;

    public AnnouncementAdminController(IAnnouncementAdminAppService announcementAppService)
    {
        _announcementAppService = announcementAppService;
    }

    [HttpGet("{id}")]
    public async Task<AnnouncementDetailDto> GetAsync(Guid id)
    {
        return await _announcementAppService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<AnnouncementListDto>> GetListAsync(GetAnnouncementsInput input)
    {
        return await _announcementAppService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<AnnouncementDetailDto> CreateAsync(AnnouncementCreateDto input)
    {
        return await _announcementAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<AnnouncementDetailDto> UpdateAsync(Guid id, AnnouncementUpdateDto input)
    {
        return await _announcementAppService.UpdateAsync(id, input);
    }

    [HttpPatch("{id}/publish")]
    public async Task PublishAsync(Guid id)
    {
        await _announcementAppService.PublishAsync(id);
    }

    [HttpPatch("{id}/unpublish")]
    public async Task UnpublishAsync(Guid id)
    {
        await _announcementAppService.UnpublishAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await _announcementAppService.DeleteAsync(id);
    }
}