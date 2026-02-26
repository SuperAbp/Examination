using Microsoft.AspNetCore.SignalR;
using SuperAbp.Exam.Notifications.Event;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace SuperAbp.Exam;

public class NotificationSignalRHandler(IHubContext<NotificationHub> hubContext) : IDistributedEventHandler<NotificationEto>, ITransientDependency
{
    protected IHubContext<NotificationHub> HubContext { get; } = hubContext;

    public virtual async Task HandleEventAsync(NotificationEto eventData)
    {
        await HubContext.Clients.User(eventData.ReceiverId.ToString().ToLower())
            .SendAsync("ReceiveNotification", new
            {
                Id = eventData.Id,
                Type = eventData.Type,
                Data = eventData.Data,
                CreationTime = eventData.CreationTime,
                RelatedEntityId = eventData.RelatedEntityId,
                RelatedEntityType = eventData.RelatedEntityType
            });
    }
}
