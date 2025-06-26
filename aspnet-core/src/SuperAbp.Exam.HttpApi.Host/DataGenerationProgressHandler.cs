using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam;

public class DataGenerationProgressHandler(IHubContext<ProgressHub> hubContext) : ILocalEventHandler<DataGenerationProgressUpdatedEto>, ITransientDependency
{
    protected IHubContext<ProgressHub> HubContext { get; } = hubContext;

    public virtual async Task HandleEventAsync(DataGenerationProgressUpdatedEto eventData)
    {
        await HubContext.Clients.User(eventData.UserId.ToString().ToLower())
            .SendAsync("ReceiveProgress", eventData.Progress);
    }
}