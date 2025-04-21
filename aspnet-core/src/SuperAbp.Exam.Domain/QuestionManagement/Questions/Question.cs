using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 题目
/// </summary>
public class Question : FullAuditedAggregateRoot<Guid>
{
    protected Question()
    {
        Content = String.Empty;
    }

    [SetsRequiredMembers]
    protected internal Question(Guid id, Guid questionBankId, QuestionType questionType, string content) :
        base(id)
    {
        QuestionBankId = questionBankId;
        QuestionType = questionType;
        Content = content;
    }

    public QuestionType QuestionType { get; private set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; internal set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string? Analysis { get; set; }

    /// <summary>
    /// 所属题库
    /// </summary>
    public Guid QuestionBankId { get; set; }

    /// <summary>
    /// 所属分类
    /// </summary>
    public Guid QuestionCategoryId { get; set; }
}