namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 已作废状态
/// </summary>
public class ExamInvalidatedState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Invalidated;
    
    // 已作废状态是终态，不允许任何状态转换
}
