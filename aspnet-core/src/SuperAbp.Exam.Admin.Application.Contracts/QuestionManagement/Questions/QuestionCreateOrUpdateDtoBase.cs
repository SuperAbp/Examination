using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateOrUpdateDtoBase
{
    /// <summary>
    /// 题干
    /// </summary>
    [Required]
    [StringLength(QuestionConsts.MaxContentLength)]
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    [StringLength(QuestionConsts.MaxAnalysisLength)]
    public string? Analysis { get; set; }

    /// <summary>
    /// 所属题库
    /// </summary>
    [Required]
    public Guid QuestionBankId { get; set; }

    public IReadOnlyList<Guid>? KnowledgePointIds { get; set; }
}