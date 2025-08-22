using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using SuperAbp.Exam.MistakesReviews.Events;

namespace SuperAbp.Exam.TrainingManagement;

/// <summary>
/// 训练
/// </summary>
public class Training : AggregateRoot<Guid>, IHasCreationTime
{
    protected Training()
    { }

    public Training(Guid id, Guid userId, Guid questionBankId, Guid questionId, TrainingSource trainingSource) : base(id)
    {
        UserId = userId;
        QuestionBankId = questionBankId;
        QuestionId = questionId;
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

    public bool Right { get; internal set; }

    public TrainingSource TrainingSource { get; set; }

    public DateTime CreationTime { get; set; }

    public void SetRight(bool right)
    {
        Right = right;
        AddLocalEvent(new AnsweredQuestionEvent(QuestionId, UserId, right));
    }
}