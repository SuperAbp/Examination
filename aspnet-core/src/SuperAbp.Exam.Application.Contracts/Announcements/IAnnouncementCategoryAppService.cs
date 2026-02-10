using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告分类查询
/// </summary>
public interface IAnnouncementCategoryAppService : IApplicationService
{
    /// <summary>
    /// 详情
    /// </summary>
    Task<AnnouncementCategoryDto> GetAsync(Guid id);

    /// <summary>
    /// 列表
    /// </summary>
    Task<ListResultDto<AnnouncementCategoryDto>> GetListAsync();
}
