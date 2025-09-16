using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.MistakesReviews;

/// <summary>
/// 错题本
/// </summary>
public class MistakesReview : AggregateRoot<Guid>, IHasCreationTime, IMultiTenant
{
    protected MistakesReview()
    {
    }

    [SetsRequiredMembers]
    public MistakesReview(Guid id, Guid questionId, Guid userId) :
        base(id)
    {
        QuestionId = questionId;
        UserId = userId;
        ErrorCount = 1;
    }

    public Guid QuestionId { get; set; }

    public Guid UserId { get; set; }

    public int ErrorCount { get; set; }

    public DateTime CreationTime { get; set; }
    public Guid? TenantId { get; set; }
}