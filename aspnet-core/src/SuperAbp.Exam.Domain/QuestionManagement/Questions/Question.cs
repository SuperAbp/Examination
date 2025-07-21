using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
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

        Answers = [];
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

    public List<QuestionAnswer> Answers { get; private set; }

    public Question AddAnswer(Guid answerId, string content, bool right, int sort = 0, string? analysis = null)
    {
        if (Answers.Any(x => x.Content == content))
        {
            throw new QuestionAnswerContentAlreadyExistException(content);
        }

        QuestionAnswer answer = new(answerId, Id, content, right, sort, analysis);
        Answers.Add(answer);

        return this;
    }

    public Question UpdateAnswer(Guid answerId, string content, bool right, int sort, string? analysis)
    {
        if (Answers.Any(a => a.Content == content && a.Id != answerId))
        {
            throw new QuestionAnswerContentAlreadyExistException(content);
        }

        QuestionAnswer? answer = Answers.SingleOrDefault(a => a.Id == answerId);
        if (answer is null)
        {
            throw new EntityNotFoundException(typeof(QuestionAnswer));
        }

        answer.Content = content;
        answer.Right = right;
        answer.Sort = sort;
        answer.Analysis = analysis;

        return this;
    }

    public Question RemoveAnswer(Guid answerId)
    {
        QuestionAnswer? answer = Answers.SingleOrDefault(a => a.Id == answerId);
        if (answer is null)
        {
            throw new EntityNotFoundException(typeof(QuestionAnswer));
        }
        Answers.Remove(answer);
        return this;
    }
}