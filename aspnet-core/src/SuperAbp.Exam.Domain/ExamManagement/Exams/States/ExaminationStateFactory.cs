using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 考试状态工厂
/// </summary>
public static class ExaminationStateFactory
{
    private static readonly Dictionary<ExaminationStatus, IExaminationState> _states = new()
    {
        { ExaminationStatus.Draft, new DraftState() },
        { ExaminationStatus.Published, new PublishedState() },
        { ExaminationStatus.Grading, new GradingState() },
        { ExaminationStatus.Completed, new CompletedState() },
        { ExaminationStatus.Cancelled, new CancelledState() },
        { ExaminationStatus.Invalidated, new ExamInvalidatedState() }
    };

    public static IExaminationState GetState(ExaminationStatus status)
    {
        if (_states.TryGetValue(status, out var state))
        {
            return state;
        }

        throw new ArgumentException($"Unknown examination status: {status}", nameof(status));
    }
}
