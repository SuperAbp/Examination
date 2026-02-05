using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告查询
/// </summary>
public interface IAnnouncementAppService : IApplicationService
{
    /// <summary>
    /// 详情
    /// </summary>
    Task<AnnouncementDto> GetAsync(Guid id);

    /// <summary>
    /// 列表
    /// </summary>
    Task<ListResultDto<AnnouncementDto>> GetEffectiveListAsync(Guid? categoryId = null);
}
