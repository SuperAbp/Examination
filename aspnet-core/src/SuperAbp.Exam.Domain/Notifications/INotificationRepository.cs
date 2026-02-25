using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知仓储接口
/// </summary>
public interface INotificationRepository : IRepository<Notification, Guid>
{
    /// <summary>
    /// 获取用户的通知列表
    /// </summary>
    Task<List<Notification>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? receiverId = null,
        NotificationType? type = null,
        bool? isRead = null,
        string? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户的通知数量
    /// </summary>
    Task<long> GetCountAsync(
        Guid? receiverId,
        NotificationType? type = null,
        bool? isRead = null,
        string? filter = null,
        CancellationToken cancellationToken = default);
}