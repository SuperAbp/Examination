using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 查询条件
/// </summary>
public class GetQuestionsInput : PagedAndSortedResultRequestDto
{
    public GetQuestionsInput()
    {
        QuestionBankIds = [];
    }

    /// <summary>
    /// 题干
    /// </summary>
    public string? Content { get; set; }

    public int? QuestionType { get; set; }

    public List<Guid> QuestionBankIds { get; set; }
    public List<Guid>? ExcludeIds { get; set; }
    public List<Guid>? IncludeIds { get; set; }
}