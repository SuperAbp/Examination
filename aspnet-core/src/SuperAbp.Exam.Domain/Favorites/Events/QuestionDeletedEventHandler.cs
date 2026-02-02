using SuperAbp.Exam.QuestionManagement.Questions;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace SuperAbp.Exam.Favorites.Events;

/// <summary>
/// 题目删除事件处理器 - 清理收藏关联数据
/// </summary>
public class QuestionDeletedEventHandler(
    IFavoriteRepository favoriteRepository)
    : ILocalEventHandler<QuestionDeletedEvent>, ITransientDependency
{
    public async Task HandleEventAsync(QuestionDeletedEvent eventData)
    {
        await favoriteRepository.DeleteByQuestionIdAsync(eventData.QuestionId);
    }
}
