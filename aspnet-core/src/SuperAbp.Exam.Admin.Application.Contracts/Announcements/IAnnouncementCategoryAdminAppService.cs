using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Announcements;

/// <summary>
/// 公告分类管理
/// </summary>
public interface IAnnouncementCategoryAdminAppService : IApplicationService
{
    /// <summary>
    /// 获取详情
    /// </summary>
    Task<AnnouncementCategoryDetailDto> GetAsync(Guid id);

    /// <summary>
    /// 获取列表
    /// </summary>
    Task<ListResultDto<AnnouncementCategoryListDto>> GetListAsync();

    /// <summary>
    /// 创建
    /// </summary>
    Task<AnnouncementCategoryDetailDto> CreateAsync(AnnouncementCategoryCreateDto input);

    /// <summary>
    /// 更新
    /// </summary>
    Task<AnnouncementCategoryDetailDto> UpdateAsync(Guid id, AnnouncementCategoryUpdateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task DeleteAsync(Guid id);
}