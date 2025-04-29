using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.TrainingManagement;

/// <summary>
/// 训练
/// </summary>
public class Training : AggregateRoot<Guid>, IHasCreationTime
{
    protected Training()
    { }

    public Training(Guid id, Guid userId, Guid questionBankId, Guid questionId, bool right, TrainingSource trainingSource) : base(id)
    {
        UserId = userId;
        QuestionBankId = questionBankId;
        QuestionId = questionId;
        Right = right;
        TrainingSource = trainingSource;
    }

    public Guid UserId { get; set; }

    /// <summary>
    /// 题库Id
    /// </summary>
    public Guid QuestionBankId { get; set; }

    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; set; }

    public bool Right { get; set; }

    public TrainingSource TrainingSource { get; set; }

    public DateTime CreationTime { get; protected set; }
}