using SuperAbp.Exam.QuestionManagement.Questions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.Mistakes.Events;

/// <summary>
/// 题目删除事件处理器 - 清理错题本关联数据
/// </summary>
public class QuestionDeletedEventHandler(
    IMistakeRepository mistakeRepository)
    : ILocalEventHandler<QuestionDeletedEvent>, ITransientDependency
{
    public async Task HandleEventAsync(QuestionDeletedEvent eventData)
    {
        await mistakeRepository.DeleteByQuestionIdAsync(eventData.QuestionId);
    }
}
