using System;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

public class QuestionKnowledgePoint : Entity<Guid>
{
    protected QuestionKnowledgePoint()
    {
    }

    public QuestionKnowledgePoint(Guid questionId, Guid knowledgePointId)
    {
        QuestionId = questionId;
        KnowledgePointId = knowledgePointId;
    }

    public Guid QuestionId { get; set; }
    public Guid KnowledgePointId { get; set; }

    public override object[] GetKeys()
    {
        return [QuestionId, KnowledgePointId];
    }
}