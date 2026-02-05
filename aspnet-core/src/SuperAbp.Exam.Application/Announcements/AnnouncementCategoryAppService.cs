using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Announcements;

public class AnnouncementCategoryAppService : ApplicationService, IAnnouncementCategoryAppService
{
    private readonly IAnnouncementCategoryRepository _repository;

    public AnnouncementCategoryAppService(IAnnouncementCategoryRepository repository)
    {
        _repository = repository;
    }

    public virtual async Task<AnnouncementCategoryDto> GetAsync(Guid id)
    {
        var category = await _repository.GetAsync(id);
        return ObjectMapper.Map<AnnouncementCategory, AnnouncementCategoryDto>(category);
    }

    public virtual async Task<ListResultDto<AnnouncementCategoryDto>> GetListAsync()
    {
        var categories = await _repository.GetListAsync();

        return new ListResultDto<AnnouncementCategoryDto>(
            ObjectMapper.Map<List<AnnouncementCategory>, List<AnnouncementCategoryDto>>(categories)
        );
    }
}
