using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateOrUpdateDtoBase
{
    /// <summary>
    /// 题干
    /// </summary>
    public required string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// 所属题库
    /// </summary>
    public Guid QuestionBankId { get; set; }

    public IReadOnlyList<Guid>? KnowledgePointIds { get; set; }
}