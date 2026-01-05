using System;

namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 等待状态
/// </summary>
public class WaitingState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.Waiting;

    public override void Start(UserExam userExam, DateTime startTime)
    {
        userExam.StartTime = startTime;
        ChangeState(userExam, new InProgressState());
    }

    public override void Invalidate(UserExam userExam)
    {
        ChangeState(userExam, new InvalidatedState());
    }
}
