using System;

namespace SuperAbp.Exam.Blazor.Model;

public class QuestionAnswerSelectEventArgs
{
    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
}