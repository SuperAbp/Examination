using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.Announcements;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Announcements;

/// <summary>
/// 公告管理
/// </summary>
public interface IAnnouncementAdminAppService : IApplicationService
{
    /// <summary>
    /// 获取详情
    /// </summary>
    Task<AnnouncementDetailDto> GetAsync(Guid id);

    /// <summary>
    /// 获取列表
    /// </summary>
    Task<PagedResultDto<AnnouncementListDto>> GetListAsync(GetAnnouncementsInput input);

    /// <summary>
    /// 创建
    /// </summary>
    Task<AnnouncementDetailDto> CreateAsync(AnnouncementCreateDto input);

    /// <summary>
    /// 更新
    /// </summary>
    Task<AnnouncementDetailDto> UpdateAsync(Guid id, AnnouncementUpdateDto input);

    /// <summary>
    /// 发布
    /// </summary>
    Task PublishAsync(Guid id);

    /// <summary>
    /// 下架
    /// </summary>
    Task UnpublishAsync(Guid id);

    /// <summary>
    /// 删除
    /// </summary>
    Task DeleteAsync(Guid id);
}