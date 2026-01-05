using System;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 草稿状态
/// </summary>
public class DraftState : ExaminationStateBase
{
    public override ExaminationStatus Status => ExaminationStatus.Draft;

    public override void Publish(Examination examination)
    {
        // 草稿状态可以发布
        ChangeState(examination, new PublishedState());
    }

    public override bool CanUpdate()
    {
        // 草稿状态可以更新
        return true;
    }
}
