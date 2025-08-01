using System;

namespace SuperAbp.Exam.Blazor.Pages;

public class QuestionAnswerViewModel
{
    public Guid Id { get; set; }

    /// <summary>
    /// 是否正确
    /// </summary>
    public bool? Right { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }
}