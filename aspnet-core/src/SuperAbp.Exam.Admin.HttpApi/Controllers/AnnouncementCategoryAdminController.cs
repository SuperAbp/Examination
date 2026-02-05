using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Announcements;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

[Route("api/announcement-categories")]
public class AnnouncementCategoryAdminController(IAnnouncementCategoryAdminAppService categoryAppService) : AbpController, IAnnouncementCategoryAdminAppService
{
    protected IAnnouncementCategoryAdminAppService CategoryAppService { get; } = categoryAppService;

    [HttpGet("{id}")]
    public virtual async Task<AnnouncementCategoryDetailDto> GetAsync(Guid id)
    {
        return await CategoryAppService.GetAsync(id);
    }

    [HttpGet]
    public virtual async Task<ListResultDto<AnnouncementCategoryListDto>> GetListAsync()
    {
        return await CategoryAppService.GetListAsync();
    }

    [HttpPost]
    public virtual async Task<AnnouncementCategoryDetailDto> CreateAsync(AnnouncementCategoryCreateDto input)
    {
        return await CategoryAppService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public virtual async Task<AnnouncementCategoryDetailDto> UpdateAsync(Guid id, AnnouncementCategoryUpdateDto input)
    {
        return await CategoryAppService.UpdateAsync(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task DeleteAsync(Guid id)
    {
        await CategoryAppService.DeleteAsync(id);
    }
}