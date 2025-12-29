using System;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamWithRanking
{
    public Guid UserExamId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public decimal TotalScore { get; set; }

    public decimal TotalCount { get; set; }

    public bool? IsPassed { get; set; }

    public DateTime? FinishedTime { get; set; }
}