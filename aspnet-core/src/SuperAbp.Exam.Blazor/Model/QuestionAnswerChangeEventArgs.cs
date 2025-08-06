using System;

namespace SuperAbp.Exam.Blazor.Model;

public class QuestionAnswerChangeEventArgs
{
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}