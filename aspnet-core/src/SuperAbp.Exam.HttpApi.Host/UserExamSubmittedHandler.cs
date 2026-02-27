using Microsoft.AspNetCore.SignalR;
using SuperAbp.Exam.ExamManagement.UserExams;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam;

public class UserExamSubmittedHandler(IHubContext<DataGenerationProgressHub> hubContext) : ILocalEventHandler<UserExamSubmittedEto>, ITransientDependency
{
    protected IHubContext<DataGenerationProgressHub> HubContext { get; } = hubContext;

    public virtual async Task HandleEventAsync(UserExamSubmittedEto eventData)
    {
        await HubContext.Clients.User(eventData.UserId.ToString().ToLower())
            .SendAsync("Submitted");
    }
}