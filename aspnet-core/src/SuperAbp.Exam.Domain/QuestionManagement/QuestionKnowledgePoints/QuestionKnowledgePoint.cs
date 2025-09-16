using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

public class QuestionKnowledgePoint : Entity<Guid>, IMultiTenant
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

    public Guid? TenantId { get; set; }
}