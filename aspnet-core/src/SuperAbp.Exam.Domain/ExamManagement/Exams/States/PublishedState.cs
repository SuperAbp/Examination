using System;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 已发布状态
/// </summary>
public class PublishedState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Published;

    public override void Cancel(Examination examination)
    {
        ChangeState(examination, new CancelledState());
    }

    public override void Terminate(Examination examination, DateTime endTime)
    {
        examination.setEndTime(endTime);
        ChangeState(examination, new GradingState());
    }
}