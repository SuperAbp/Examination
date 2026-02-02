using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.PaperManagement.Papers;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.PaperManagement.Events;

/// <summary>
/// 知识点删除事件处理器
/// </summary>
public class KnowledgePointDeletedEventHandler(
    PaperManager paperManager)
    : ILocalEventHandler<KnowledgePointDeletedEvent>, ITransientDependency
{
    public async Task HandleEventAsync(KnowledgePointDeletedEvent eventData)
    {
        await paperManager.RemoveKnowledgePointFromAllPapersAsync(
            eventData.KnowledgePointId,
            eventData.TenantId);
    }
}