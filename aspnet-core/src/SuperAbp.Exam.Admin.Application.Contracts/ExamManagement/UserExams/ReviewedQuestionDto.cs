using System;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

public class ReviewedQuestionDto
{
    public Guid QuestionId { get; set; }

    public bool Right { get; set; }

    /// <summary>
    /// 得分
    /// </summary>
    public decimal? Score { get; set; }

    public string? Reason { get; set; }
}