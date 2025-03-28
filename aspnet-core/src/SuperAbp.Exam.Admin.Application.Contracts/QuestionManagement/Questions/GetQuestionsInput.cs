using SuperAbp.Exam.QuestionManagement.Questions;
using System;
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

    public Guid[] QuestionRepositoryIds { get; set; } = [];
}