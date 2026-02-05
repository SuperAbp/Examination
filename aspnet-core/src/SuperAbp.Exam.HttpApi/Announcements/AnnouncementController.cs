using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Announcements;

[RemoteService(Name = "Exam")]
[Area("Exam")]
[Route("api/exam/announcements")]
public class AnnouncementController : AbpController, IAnnouncementAppService
{
    private readonly IAnnouncementAppService _announcementAppService;

    public AnnouncementController(IAnnouncementAppService announcementAppService)
    {
        _announcementAppService = announcementAppService;
    }

    [HttpGet("{id}")]
    public Task<AnnouncementDto> GetAsync(Guid id)
    {
        return _announcementAppService.GetAsync(id);
    }

    [HttpGet("effective")]
    public Task<ListResultDto<AnnouncementDto>> GetEffectiveListAsync([FromQuery] Guid? categoryId = null)
    {
        return _announcementAppService.GetEffectiveListAsync(categoryId);
    }
}
