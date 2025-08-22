using System;

namespace SuperAbp.Exam.MistakesReviews.Events;

public class AnsweredQuestionEvent(Guid questionId, Guid userId, bool right)
{
    public Guid QuestionId { get; } = questionId;
    public Guid UserId { get; } = userId;
    public bool Right { get; } = right;
}