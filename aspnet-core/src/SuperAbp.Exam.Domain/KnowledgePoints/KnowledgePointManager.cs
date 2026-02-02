using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace SuperAbp.Exam.KnowledgePoints;

public class KnowledgePointManager(
    IKnowledgePointRepository knowledgePointRepository,
    IQuestionKnowledgePointRepository questionKnowledgePointRepository,
    ILocalEventBus localEventBus) : DomainService
{
    protected IKnowledgePointRepository KnowledgePointRepository => knowledgePointRepository;
    protected IQuestionKnowledgePointRepository QuestionKnowledgePointRepository => questionKnowledgePointRepository;

    public virtual async Task DeleteAsync(KnowledgePoint knowledgePoint)
    {
        await QuestionKnowledgePointRepository.DeleteByKnowledgePointIdAsync(knowledgePoint.Id);

        await localEventBus.PublishAsync(new KnowledgePointDeletedEvent
        {
            KnowledgePointId = knowledgePoint.Id,
            TenantId = knowledgePoint.TenantId
        });

        await KnowledgePointRepository.DeleteAsync(knowledgePoint);
    }
}