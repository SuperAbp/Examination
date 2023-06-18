using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 查询条件
/// </summary>
public class GetQuestionsInput
{
    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    public QuestionType? QuestionType { get; set; }

    public Guid QuestionRepositoryId { get; set; }
}