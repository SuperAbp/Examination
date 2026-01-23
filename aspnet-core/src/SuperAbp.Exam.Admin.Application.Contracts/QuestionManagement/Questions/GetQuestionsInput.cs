using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 查询条件
/// </summary>
public class GetQuestionsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 题干
    /// </summary>
    public string? Content { get; set; }

    public int? QuestionType { get; set; }

    public Guid[] QuestionBankIds { get; set; } = [];
    public Guid? KnowledgePointId { get; set; }
    public List<Guid>? ExcludeIds { get; set; }
}