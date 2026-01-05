using System;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 评分中状态
/// </summary>
public class GradingState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Grading;

    public override void Complete(Examination examination)
    {
        // 评分中状态可以完成
        ChangeState(examination, new CompletedState());
    }
}
