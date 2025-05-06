using System;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamWithUser
{
    public Guid UserId { get; set; }

    public int TotalCount { get; set; }

    public decimal MaxScore { get; set; }
}