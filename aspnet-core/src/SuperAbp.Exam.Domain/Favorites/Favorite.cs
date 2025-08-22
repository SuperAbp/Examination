using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.Favorites;

/// <summary>
/// 我的收藏夹
/// </summary>
public class Favorite : AggregateRoot<Guid>, IHasCreationTime
{
    protected Favorite()
    {
    }

    [SetsRequiredMembers]
    public Favorite(Guid id, Guid questionId, Guid creatorId, DateTime creationTime) :
        base(id)
    {
        QuestionId = questionId;
        CreatorId = creatorId;
        CreationTime = creationTime;
    }

    public Guid QuestionId { get; set; }

    public Guid CreatorId { get; set; }

    public DateTime CreationTime { get; }
}