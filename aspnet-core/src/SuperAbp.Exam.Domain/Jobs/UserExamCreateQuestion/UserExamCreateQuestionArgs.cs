using System;

namespace SuperAbp.Exam.Jobs.UserExamCreateQuestion;

public class UserExamCreateQuestionArgs
{
    public Guid UserExamId { get; set; }
    public Guid? TenantId { get; set; }
}