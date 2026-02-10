using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Announcements;

[Authorize(ExamPermissions.AnnouncementCategories.Default)]
public class AnnouncementCategoryAdminAppService(IAnnouncementCategoryRepository repository) : ApplicationService, IAnnouncementCategoryAdminAppService
{
    protected IAnnouncementCategoryRepository Rpository { get; } = repository;

    public virtual async Task<AnnouncementCategoryDetailDto> GetAsync(Guid id)
    {
        var category = await Rpository.GetAsync(id);
        return ObjectMapper.Map<AnnouncementCategory, AnnouncementCategoryDetailDto>(category);
    }

    public virtual async Task<ListResultDto<AnnouncementCategoryListDto>> GetListAsync()
    {
        var categories = await Rpository.GetListAsync();

        return new ListResultDto<AnnouncementCategoryListDto>(
            ObjectMapper.Map<List<AnnouncementCategory>, List<AnnouncementCategoryListDto>>(categories)
        );
    }

    [Authorize(ExamPermissions.AnnouncementCategories.Create)]
    public virtual async Task<AnnouncementCategoryDetailDto> CreateAsync(AnnouncementCategoryCreateDto input)
    {
        var category = new AnnouncementCategory(
            GuidGenerator.Create(),
            input.Name,
            input.Sort,
            input.Remark
        );

        await Rpository.InsertAsync(category);
        return ObjectMapper.Map<AnnouncementCategory, AnnouncementCategoryDetailDto>(category);
    }

    [Authorize(ExamPermissions.AnnouncementCategories.Update)]
    public virtual async Task<AnnouncementCategoryDetailDto> UpdateAsync(Guid id, AnnouncementCategoryUpdateDto input)
    {
        var category = await Rpository.GetAsync(id);

        category.Name = input.Name;
        category.Sort = input.Sort;
        category.Remark = input.Remark;

        await Rpository.UpdateAsync(category);
        return ObjectMapper.Map<AnnouncementCategory, AnnouncementCategoryDetailDto>(category);
    }

    [Authorize(ExamPermissions.AnnouncementCategories.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await Rpository.DeleteAsync(id);
    }
}