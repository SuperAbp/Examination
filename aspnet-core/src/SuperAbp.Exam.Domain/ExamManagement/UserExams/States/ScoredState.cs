namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 已出分状态
/// </summary>
public class ScoredState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.Scored;

    // 已出分状态是终态
}
