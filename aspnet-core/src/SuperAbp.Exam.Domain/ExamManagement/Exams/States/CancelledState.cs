namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 已取消状态
/// </summary>
public class CancelledState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Cancelled;

    // 已取消状态是终态，不允许任何状态转换
}
