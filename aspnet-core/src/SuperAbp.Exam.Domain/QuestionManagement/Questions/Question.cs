using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 题目
/// </summary>
public class Question : FullAuditedAggregateRoot<Guid>, IMultiTenant
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

        Options = [];
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

    public Guid? TenantId { get; set; }

    public List<QuestionOption> Options { get; private set; }

    public Question AddAnswer(Guid answerId, string content, bool right, int sort = 0, string? analysis = null)
    {
        if (Options.Any(x => x.Content == content))
        {
            throw new QuestionOptionContentAlreadyExistException(content);
        }

        QuestionOption answer = new(answerId, Id, content, right, sort, analysis);
        Options.Add(answer);

        return this;
    }

    public Question UpdateAnswer(Guid answerId, string content, bool right, int sort, string? analysis)
    {
        if (Options.Any(a => a.Content == content && a.Id != answerId))
        {
            throw new QuestionOptionContentAlreadyExistException(content);
        }

        QuestionOption? answer = Options.SingleOrDefault(a => a.Id == answerId);
        if (answer is null)
        {
            throw new EntityNotFoundException(typeof(QuestionOption));
        }

        answer.Content = content;
        answer.Right = right;
        answer.Sort = sort;
        answer.Analysis = analysis;

        return this;
    }

    public Question RemoveAnswer(Guid answerId)
    {
        QuestionOption? answer = Options.SingleOrDefault(a => a.Id == answerId);
        if (answer is null)
        {
            throw new EntityNotFoundException(typeof(QuestionOption));
        }
        Options.Remove(answer);
        return this;
    }
}