using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.Questions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.PaperManagement.Events;

/// <summary>
/// 题目删除事件处理器 - 清理试卷关联数据
/// </summary>
public class QuestionDeletedEventHandler(
    PaperManager paperManager)
    : ILocalEventHandler<QuestionDeletedEvent>, ITransientDependency
{
    public async Task HandleEventAsync(QuestionDeletedEvent eventData)
    {
        // 通过 Manager 的方法处理，确保聚合根的完整性
        await paperManager.RemoveQuestionFromAllPapersAsync(eventData.QuestionId, eventData.TenantId);
    }
}
