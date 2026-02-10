using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp;

namespace SuperAbp.Exam.Announcements;

public class AnnouncementAppService : ApplicationService, IAnnouncementAppService
{
    private readonly IAnnouncementRepository _repository;

    public AnnouncementAppService(IAnnouncementRepository repository)
    {
        _repository = repository;
    }

    public virtual async Task<AnnouncementDetailDto> GetAsync(Guid id)
    {
        var announcement = await _repository.GetAsync(id);

        if (!announcement.IsEffective(Clock.Now))
        {
            throw new BusinessException("Announcement not found or not effective");
        }

        return ObjectMapper.Map<Announcement, AnnouncementDetailDto>(announcement);
    }

    public virtual async Task<ListResultDto<AnnouncementListDto>> GetListAsync(Guid? categoryId = null)
    {
        var items = categoryId.HasValue
            ? await _repository.GetEffectiveListByCategoryIdAsync(categoryId.Value, Clock.Now)
            : await _repository.GetEffectiveListAsync(Clock.Now);

        var dtos = ObjectMapper.Map<List<Announcement>, List<AnnouncementListDto>>(items);

        return new ListResultDto<AnnouncementListDto>(dtos);
    }
}