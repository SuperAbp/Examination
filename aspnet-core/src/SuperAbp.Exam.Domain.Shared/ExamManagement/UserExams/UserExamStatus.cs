using Ardalis.SmartEnum;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 状态
/// </summary>
public class UserExamStatus : SmartEnum<UserExamStatus>
{
    public static readonly UserExamStatus NotStarted = new UserExamStatus(nameof(NotStarted), 0);
    public static readonly UserExamStatus InProgress = new UserExamStatus(nameof(InProgress), 1);
    public static readonly UserExamStatus Submitted = new UserExamStatus(nameof(Submitted), 2);
    public static readonly UserExamStatus TimeoutAutoSubmitted = new UserExamStatus(nameof(TimeoutAutoSubmitted), 3);
    public static readonly UserExamStatus Reviewed = new UserExamStatus(nameof(Reviewed), 4);
    public static readonly UserExamStatus Scored = new UserExamStatus(nameof(Scored), 5);
    public static readonly UserExamStatus Invalidated = new UserExamStatus(nameof(Invalidated), 6);

    public UserExamStatus(string name, int value) : base(name, value)
    {
    }
}