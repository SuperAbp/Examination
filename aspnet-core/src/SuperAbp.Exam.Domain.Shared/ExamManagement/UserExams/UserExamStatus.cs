using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 状态
/// </summary>
public class UserExamStatus : SmartEnum<UserExamStatus>
{
    public static readonly UserExamStatus InProgress = new(nameof(InProgress), 0);
    public static readonly UserExamStatus Submitted = new(nameof(Submitted), 1);
    public static readonly UserExamStatus Scored = new(nameof(Scored), 2);
    public static readonly UserExamStatus Invalidated = new(nameof(Invalidated), 3);

    public UserExamStatus(string name, int value) : base(name, value)
    {
    }
}