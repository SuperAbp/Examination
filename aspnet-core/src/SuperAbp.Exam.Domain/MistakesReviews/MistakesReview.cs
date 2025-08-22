using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.MistakesReviews;

/// <summary>
/// 错题本
/// </summary>
public class MistakesReview : AggregateRoot<Guid>, IHasCreationTime
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
    }

    public Guid QuestionId { get; set; }

    public Guid UserId { get; set; }

    public int ErrorCount { get; set; }

    public DateTime CreationTime { get; set; }
}