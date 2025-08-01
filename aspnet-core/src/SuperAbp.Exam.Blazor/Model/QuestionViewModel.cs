using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Blazor.Model;

public class QuestionViewModel
{
    public Guid Id { get; set; }

    public int QuestionType { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }

    public IReadOnlyList<string> KnowledgePoints { get; set; } = [];
    public IReadOnlyList<QuestionAnswerDto> Answers { get; set; } = [];
}