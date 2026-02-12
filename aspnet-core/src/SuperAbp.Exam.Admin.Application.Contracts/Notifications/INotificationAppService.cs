using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Notifications;

/// <summary>
/// 通知应用服务接口
/// </summary>
public interface INotificationAppService : IApplicationService
{
    /// <summary>
    /// 获取当前用户通知列表
    /// </summary>
    Task<PagedResultDto<NotificationMyListDto>> GetMyListAsync(GetMyNotificationsInput input);

    /// <summary>
    /// 标记为已读
    /// </summary>
    Task MarkAsReadAsync(Guid id);

    /// <summary>
    /// 全部标记为已读
    /// </summary>
    Task MarkAllAsReadAsync();

    /// <summary>
    /// 获取未读数量
    /// </summary>
    Task<long> GetUnreadCountAsync();

    /// <summary>
    /// 获取所有通知列表（管理员）
    /// </summary>
    Task<PagedResultDto<NotificationListDto>> GetListAsync(GetNotificationsInput input);
}