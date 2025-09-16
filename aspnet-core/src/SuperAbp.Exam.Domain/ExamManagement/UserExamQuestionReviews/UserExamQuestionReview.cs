using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestionReviews;

/// <summary>
/// 用户考试题目评审
/// </summary>
public class UserExamQuestionReview : FullAuditedEntity<Guid>, IMultiTenant
{
    protected UserExamQuestionReview()
    {
    }

    public UserExamQuestionReview(Guid id, Guid userExamQuestionId, bool right, decimal score, string? reason) : base(id)
    {
        UserExamQuestionId = userExamQuestionId;
        Right = right;
        Score = score;
        Reason = reason;
    }

    public Guid UserExamQuestionId { get; set; }
    public bool Right { get; set; }
    public decimal Score { get; set; }
    public string? Reason { get; set; }

    public UserExamQuestion Question { get; set; }
    public Guid? TenantId { get; set; }
}