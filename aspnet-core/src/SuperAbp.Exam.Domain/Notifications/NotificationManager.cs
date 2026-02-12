using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知管理器
/// </summary>
public class NotificationManager : DomainService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationPublisher _publisher;

    public NotificationManager(
        INotificationRepository notificationRepository,
        INotificationPublisher publisher)
    {
        _notificationRepository = notificationRepository;
        _publisher = publisher;
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
        var jsonData = data is null ? null : JsonSerializer.Serialize(data);
        List<Notification> notifications = receiverIds
            .Select(receiverId => CreateNotification(type, relatedEntityId, relatedEntityType, jsonData, receiverId))
            .ToList();

        await _notificationRepository.InsertManyAsync(notifications);

        foreach (var notification in notifications)
        {
            await _publisher.PublishAsync(notification);
        }

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