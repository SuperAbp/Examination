using System;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 查询条件
/// </summary>
public class GetQuestionsInput
{
    /// <summary>
    /// 题干
    /// </summary>
    public string? Content { get; set; }

    public int? QuestionType { get; set; }

    public bool IsFavorite { get; set; }

    public Guid? QuestionRepositoryId { get; set; }
}