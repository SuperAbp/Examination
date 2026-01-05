namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 已完成状态
/// </summary>
public class CompletedState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Completed;

    public override void Invalidate(Examination examination)
    {
        // 已完成状态可以作废
        ChangeState(examination, new ExamInvalidatedState());
    }
}
