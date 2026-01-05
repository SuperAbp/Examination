namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 无效状态
/// </summary>
public class InvalidatedState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.Invalidated;

    // 无效状态是终态
}
