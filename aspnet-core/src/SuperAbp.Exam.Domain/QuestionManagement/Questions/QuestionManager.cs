using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionManager(IQuestionRepository questionRepository, IQuestionKnowledgePointRepository questionKnowledgePointRepository) : DomainService
{
    protected IQuestionRepository QuestionRepository { get; } = questionRepository;

    protected IQuestionKnowledgePointRepository QuestionKnowledgePointRepository { get; } =
        questionKnowledgePointRepository;

    public async Task<List<Guid>> GetKnowledgePointIdsAsync(Guid questionId)
    {
        List<QuestionKnowledgePoint> points = await questionKnowledgePointRepository.GetByQuestionIdAsync(questionId);
        return points.Select(p => p.KnowledgePointId).ToList();
    }

    public virtual async Task<Question> CreateAsync(Guid questionBankId, QuestionType questionType, string content)
    {
        await CheckContentAsync(content);

        return new Question(GuidGenerator.Create(), questionBankId, questionType, content);
    }

    public virtual async Task SetKnowledgePointAsync(Question question, IEnumerable<Guid> knowledgePointIds)
    {
        List<QuestionKnowledgePoint> knowledgePoints = await QuestionKnowledgePointRepository.GetByQuestionIdAsync(question.Id);
        IEnumerable<Guid> currentKnowledgePointIds = knowledgePoints.Select(kp => kp.KnowledgePointId);
        List<QuestionKnowledgePoint> removeKnowledgePoints = knowledgePoints
            .Where(k => currentKnowledgePointIds
                .Except(knowledgePointIds)
                .Distinct()
                .Contains(k.KnowledgePointId)).ToList();
        await QuestionKnowledgePointRepository.DeleteManyAsync(removeKnowledgePoints);

        IEnumerable<Guid> newIds = knowledgePointIds.Except(currentKnowledgePointIds).Distinct();
        List<QuestionKnowledgePoint> newKnowledgePoints = [];
        foreach (Guid knowledgePointId in newIds)
        {
            newKnowledgePoints.Add(new(question.Id, knowledgePointId));
        }

        await QuestionKnowledgePointRepository.InsertManyAsync(newKnowledgePoints);
    }

    public virtual async Task SetContentAsync(Question question, string content)
    {
        if (content == question.Content)
        {
            return;
        }
        await CheckContentAsync(content);

        question.Content = content;
    }

    protected virtual async Task CheckContentAsync(string content)
    {
        if (await QuestionRepository.ContentExistsAsync(content))
        {
            throw new QuestionContentAlreadyExistException(content);
        }
    }
}