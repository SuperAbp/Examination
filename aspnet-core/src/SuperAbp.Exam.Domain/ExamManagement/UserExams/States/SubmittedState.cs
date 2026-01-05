namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 已提交状态
/// </summary>
public class SubmittedState : UserExamStateBase
{
    public override UserExamStatus Status => UserExamStatus.Submitted;

    public override void Score(UserExam userExam)
    {
        ChangeState(userExam, new ScoredState());
    }

    public override void Invalidate(UserExam userExam)
    {
        ChangeState(userExam, new InvalidatedState());
    }
}
