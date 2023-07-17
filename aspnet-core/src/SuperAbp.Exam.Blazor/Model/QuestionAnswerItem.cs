using System;

namespace SuperAbp.Exam.Blazor.Model;

public class QuestionAnswerItem
{
    public Guid QuestionId { get; set; }
    public string AnswerId { get; set; }
    public bool Right { get; set; }
}