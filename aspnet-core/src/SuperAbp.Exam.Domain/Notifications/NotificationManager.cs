using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知管理器
/// </summary>
public class NotificationManager : DomainService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IJsonSerializer _jsonSerializer;

    public NotificationManager(
        INotificationRepository notificationRepository,
        IJsonSerializer jsonSerializer)
    {
        _notificationRepository = notificationRepository;
        _jsonSerializer = jsonSerializer;
    }

    /// <summary>
    /// 发送通知
    /// </summary>
    public virtual async Task<List<Notification>> NotifyAsync(
        NotificationType type,
        IEnumerable<Guid> receiverIds,
        [CanBeNull] object? data = null,
        [CanBeNull] Guid? relatedEntityId = null,
        [CanBeNull] string? relatedEntityType = null)
    {
        var jsonData = data is null ? null : _jsonSerializer.Serialize(data);
        List<Notification> notifications = receiverIds
            .Select(receiverId => CreateNotification(type, relatedEntityId, relatedEntityType, jsonData, receiverId))
            .ToList();

        await _notificationRepository.InsertManyAsync(notifications);

        return notifications;
    }

    private Notification CreateNotification(NotificationType type, Guid? relatedEntityId, string? relatedEntityType, string? jsonData, Guid receiverId)
    {
        var notification = new Notification(
                        id: GuidGenerator.Create(),
                        receiverId: receiverId,
                        type: type,
                        data: jsonData,
                        relatedEntityId: relatedEntityId,
                        relatedEntityType: relatedEntityType
                    );

        return notification;
    }
}