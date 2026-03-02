using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace SuperAbp.Exam;

public class DataGenerationProgressHandler(IHubContext<NotificationHub> hubContext)
    : IDistributedEventHandler<DataGenerationProgressUpdatedEto>, ITransientDependency
{
    protected IHubContext<NotificationHub> HubContext { get; } = hubContext;

    public virtual async Task HandleEventAsync(DataGenerationProgressUpdatedEto eventData)
    {
        await HubContext.Clients.User(eventData.UserId.ToString().ToLower())
            .SendAsync("ReceiveProgress", eventData.Progress);
    }
}