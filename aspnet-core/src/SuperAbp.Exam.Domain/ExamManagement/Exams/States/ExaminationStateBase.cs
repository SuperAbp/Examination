using System;
using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 考试状态基类
/// </summary>
public abstract class ExaminationStateBase : IExaminationState
{
    public abstract ExaminationStatus Status { get; }

    public virtual void Publish(Examination examination)
    {
        throw new InvalidExamStatusException(Status);
    }

    public virtual void Cancel(Examination examination)
    {
        throw new InvalidExamStatusException(Status);
    }

    public virtual void Terminate(Examination examination, DateTime endTime)
    {
        throw new InvalidExamStatusException(Status);
    }

    public virtual void Complete(Examination examination)
    {
        throw new InvalidExamStatusException(Status);
    }

    public virtual void Invalidate(Examination examination)
    {
        throw new InvalidExamStatusException(Status);
    }

    public virtual bool CanUpdate()
    {
        return false;
    }

    protected void ChangeState(Examination examination, IExaminationState newState)
    {
        examination.SetState(newState);
    }
}