using System;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamWithUser
{
    public Guid UserExamId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public int TotalCount { get; set; }

    public decimal MaxScore { get; set; }

    public decimal TotalScore { get; set; }

    public bool? IsPassed { get; set; }

    public DateTime? FinishedTime { get; set; }
}