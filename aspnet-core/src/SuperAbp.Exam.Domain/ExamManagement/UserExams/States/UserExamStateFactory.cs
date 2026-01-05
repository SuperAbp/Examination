using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 用户考试状态工厂
/// </summary>
public static class UserExamStateFactory
{
    private static readonly Dictionary<UserExamStatus, IUserExamState> _states = new()
    {
        { UserExamStatus.Waiting, new WaitingState() },
        { UserExamStatus.InProgress, new InProgressState() },
        { UserExamStatus.Submitted, new SubmittedState() },
        { UserExamStatus.Scored, new ScoredState() },
        { UserExamStatus.Timeout, new TimeoutState() },
        { UserExamStatus.Invalidated, new InvalidatedState() }
    };

    public static IUserExamState GetState(UserExamStatus status)
    {
        if (_states.TryGetValue(status, out var state))
        {
            return state;
        }

        throw new ArgumentException($"Unknown user exam status: {status}", nameof(status));
    }
}
