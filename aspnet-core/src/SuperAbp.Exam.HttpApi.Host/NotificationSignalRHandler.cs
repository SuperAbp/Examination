using Microsoft.AspNetCore.SignalR;
using SuperAbp.Exam.Notifications;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;

namespace SuperAbp.Exam;

public class NotificationSignalRHandler(IHubContext<NotificationHub> hubContext) : IDistributedEventHandler<EntityCreatedEto<NotificationEto>>, ITransientDependency
{
    protected IHubContext<NotificationHub> HubContext { get; } = hubContext;

    public virtual async Task HandleEventAsync(EntityCreatedEto<NotificationEto> eventData)
    {
        await HubContext.Clients.User(eventData.Entity.ReceiverId.ToString().ToLower())
           .SendAsync("ReceiveNotification", new
           {
               Id = eventData.Entity.Id,
               Type = eventData.Entity.Type,
               Data = eventData.Entity.Data,
               CreationTime = eventData.Entity.CreationTime,
               RelatedEntityId = eventData.Entity.RelatedEntityId,
               RelatedEntityType = eventData.Entity.RelatedEntityType
           });
    }
}