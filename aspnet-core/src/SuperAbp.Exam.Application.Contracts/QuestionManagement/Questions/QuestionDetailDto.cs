using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 列表
/// </summary>
public class QuestionDetailDto : EntityDto<Guid>
{
    public int QuestionType { get; set; }

    public Guid QuestionBankId { get; set; }

    public IReadOnlyList<string> KnowledgePoints { get; set; } = [];

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }
}