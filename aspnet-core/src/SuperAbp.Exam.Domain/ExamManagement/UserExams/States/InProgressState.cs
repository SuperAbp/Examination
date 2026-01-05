using System;

namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 进行中状态
/// </summary>
public class InProgressState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.InProgress;

    public override void Submit(UserExam userExam, bool requireManualReview)
    {
        userExam.FinishedTime = DateTime.Now;
        if (requireManualReview)
        {
            ChangeState(userExam, new SubmittedState());
        }
        else
        {
            ChangeState(userExam, new ScoredState());
        }
    }

    public override void Timeout(UserExam userExam)
    {
        ChangeState(userExam, new TimeoutState());
    }

    public override void Invalidate(UserExam userExam)
    {
        ChangeState(userExam, new InvalidatedState());
    }

    public override bool CanAnswer()
    {
        return true;
    }
}
