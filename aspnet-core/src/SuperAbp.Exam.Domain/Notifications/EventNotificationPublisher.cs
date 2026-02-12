using System.Threading.Tasks;
using SuperAbp.Exam.Notifications.Event;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 事件发布器
/// </summary>
public class EventNotificationPublisher : INotificationPublisher, ITransientDependency
{
    private readonly IDistributedEventBus _eventBus;

    public EventNotificationPublisher(IDistributedEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task PublishAsync(Notification notification)
    {
        await _eventBus.PublishAsync(new NotificationEto
        {
            Id = notification.Id,
            ReceiverId = notification.ReceiverId,
            Type = notification.Type.Value,
            Data = notification.Data,
            CreationTime = notification.CreationTime,
            RelatedEntityId = notification.RelatedEntityId,
            RelatedEntityType = notification.RelatedEntityType
        });
    }
}