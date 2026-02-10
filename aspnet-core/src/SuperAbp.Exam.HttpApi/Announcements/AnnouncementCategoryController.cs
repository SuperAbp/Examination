using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Announcements;

[Route("api/announcement-categories")]
public class AnnouncementCategoryController : AbpController, IAnnouncementCategoryAppService
{
    private readonly IAnnouncementCategoryAppService _categoryAppService;

    public AnnouncementCategoryController(IAnnouncementCategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet("{id}")]
    public Task<AnnouncementCategoryDto> GetAsync(Guid id)
    {
        return _categoryAppService.GetAsync(id);
    }

    [HttpGet]
    public Task<ListResultDto<AnnouncementCategoryDto>> GetListAsync()
    {
        return _categoryAppService.GetListAsync();
    }
}