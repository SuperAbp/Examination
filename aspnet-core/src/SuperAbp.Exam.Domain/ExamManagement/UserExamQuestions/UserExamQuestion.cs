using System;
using System.Collections.Generic;
using SuperAbp.Exam.ExamManagement.UserExamQuestionReviews;
using SuperAbp.Exam.ExamManagement.UserExams;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions;

/// <summary>
/// 用户考试题目
/// </summary>
public class UserExamQuestion : FullAuditedEntity<Guid>
{
    protected UserExamQuestion()
    { }

    public UserExamQuestion(Guid id, Guid userExamId, Guid questionId, decimal questionScore) : base(id)
    {
        UserExamId = userExamId;
        QuestionId = questionId;
        QuestionScore = questionScore;
    }

    public Guid UserExamId { get; set; }

    public Guid QuestionId { get; set; }

    public decimal QuestionScore { get; set; }

    /// <summary>
    /// 答案
    /// </summary>
    public string? Answers { get; set; }

    /// <summary>
    /// 正确
    /// </summary>
    public bool? Right { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    public decimal? Score { get; set; }

    public string? Reason { get; set; }

    public List<UserExamQuestionReview> QuestionReviews { get; set; }

    public void Review(Guid reviewId, bool right, decimal score, string? reason)
    {
        if (score < 0)
        {
            throw new ArgumentException("分数不能为负");
        }
        QuestionReviews.Add(new UserExamQuestionReview(reviewId, Id, right, score, reason));
        Right = right;
        Score = score;
        Reason = reason;
    }
}