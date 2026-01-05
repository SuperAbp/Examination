namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 超时状态
/// </summary>
public class TimeoutState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.Timeout;

    // 超时状态是终态
}
