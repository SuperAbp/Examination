using System;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.MistakesReviews;

public class MistakeWithDetails
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public required string QuestionContent { get; set; }
    public required QuestionType QuestionType { get; set; }

    public int ErrorCount { get; set; }
    public DateTime CreationTime { get; set; }
}