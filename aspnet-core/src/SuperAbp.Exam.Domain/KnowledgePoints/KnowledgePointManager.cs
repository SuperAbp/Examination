using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.KnowledgePoints;

public class KnowledgePointManager(IKnowledgePointRepository knowledgePointRepository,
    IQuestionKnowledgePointRepository questionKnowledgePointRepository) : DomainService
{
    public IKnowledgePointRepository KnowledgePointRepository { get; } = knowledgePointRepository;
    public IQuestionKnowledgePointRepository QuestionKnowledgePointRepository { get; } = questionKnowledgePointRepository;

    public virtual async Task DeleteAsync(KnowledgePoint knowledgePoint)
    {
        await QuestionKnowledgePointRepository.DeleteByKnowledgePointIdAsync(knowledgePoint.Id);
        await KnowledgePointRepository.DeleteAsync(knowledgePoint);
    }
}