using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionDetailDto
{
    /// <summary>
    /// 题干
    /// </summary>
    public required string Content { get; set; }

    public string? Analysis { get; set; }

    public int QuestionType { get; set; }

    public List<QuestionAnswerDto> Answers { get; set; } = [];
}