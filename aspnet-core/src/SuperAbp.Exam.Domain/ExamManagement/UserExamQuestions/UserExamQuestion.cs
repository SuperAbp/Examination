using System;
using SuperAbp.Exam.ExamManagement.UserExams;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions;

/// <summary>
/// 用户考试题目
/// </summary>
public class UserExamQuestion : Entity<Guid>
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

    public UserExam UserExam { get; set; }
}