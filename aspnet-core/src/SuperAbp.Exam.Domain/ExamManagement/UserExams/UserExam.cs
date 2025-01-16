using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 用户考试
/// </summary>
public class UserExam : AggregateRoot<Guid>, IHasCreationTime
{
    protected UserExam()
    {
    }

    public UserExam(Guid id, Guid examId, Guid userId) : base(id)
    {
        UserId = userId;
        ExamId = examId;
    }

    public Guid UserId { get; protected set; }
    public Guid ExamId { get; protected set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 是否交卷
    /// </summary>
    public bool Finished { get; set; }

    /// <summary>
    /// 交卷时间
    /// </summary>
    public DateTime? FinishedTime { get; set; }

    public DateTime CreationTime { get; protected set; }
}