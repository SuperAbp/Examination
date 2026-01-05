using System;

namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 用户考试状态基类
/// </summary>
public abstract class UserExamStateBase : IUserExamState
{
    public abstract UserExamStatus Status { get; }

    public virtual void Start(UserExam userExam, DateTime startTime)
    {
        throw new InvalidUserExamStatusException(Status);
    }

    public virtual void Submit(UserExam userExam, bool requireManualReview)
    {
        throw new InvalidUserExamStatusException(Status);
    }

    public virtual void Score(UserExam userExam)
    {
        throw new InvalidUserExamStatusException(Status);
    }

    public virtual void Timeout(UserExam userExam)
    {
        throw new InvalidUserExamStatusException(Status);
    }

    public virtual void Invalidate(UserExam userExam)
    {
        throw new InvalidUserExamStatusException(Status);
    }

    public virtual bool CanAnswer()
    {
        return false;
    }

    protected void ChangeState(UserExam userExam, IUserExamState newState)
    {
        userExam.SetState(newState);
    }
}
