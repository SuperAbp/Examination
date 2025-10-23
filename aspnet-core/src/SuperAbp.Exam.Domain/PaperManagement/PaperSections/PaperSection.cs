using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.PaperQuestions;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.PaperManagement.PaperSections;

/// <summary>
/// 试卷大题
/// </summary>
public class PaperSection : Entity<Guid>, IHasCreationTime, ISoftDelete, IMultiTenant
{
    protected PaperSection()
    {
    }

    [SetsRequiredMembers]
    public PaperSection(Guid id, Guid paperId, string title, decimal scoreEach, decimal totalScore, int order, int totalCount)
        : base(id)
    {
        Id = id;
        PaperId = paperId;
        Title = title;
        ScoreEach = scoreEach;
        TotalScore = totalScore;
        Order = order;
        TotalCount = totalCount;
        PaperQuestions = [];
        PaperQuestionRules = [];
    }

    public Guid PaperId { get; set; }
    public string Title { get; set; }
    public decimal ScoreEach { get; set; }
    public decimal TotalScore { get; set; }
    public int Order { get; set; }
    public int TotalCount { get; set; }
    public string? Remark { get; set; }

    public List<PaperQuestion> PaperQuestions { get; set; }
    public List<PaperQuestionRule> PaperQuestionRules { get; set; }

    public DateTime CreationTime { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? TenantId { get; set; }

    internal PaperSection AddQuestion(Guid paperQuestionId, Guid questionId, decimal score, int order)
    {
        PaperQuestion question = new(paperQuestionId, Id, questionId)
        {
            Score = score,
            Order = order,
            TenantId = TenantId
        };

        PaperQuestions.Add(question);
        return this;
    }

    internal PaperSection UpdateQuestion(Guid paperQuestionId, decimal score, int order)
    {
        PaperQuestion question = GetPaperQuestion(paperQuestionId);
        question.Score = score;
        question.Order = order;
        return this;
    }

    public void RemoveQuestions(List<Guid> questionIds)
    {
        foreach (Guid questionId in questionIds)
        {
            RemoveQuestion(questionId);
        }
    }

    public void RemoveQuestion(Guid paperQuestionId)
    {
        PaperQuestion? question = GetPaperQuestion(paperQuestionId);
        if (question != null)
        {
            PaperQuestions.Remove(question);
        }
    }

    private PaperQuestion GetPaperQuestion(Guid paperQuestionId)
    {
        PaperQuestion? question = PaperQuestions.SingleOrDefault(q => q.Id == paperQuestionId);
        if (question == null)
        {
            throw new EntityNotFoundException(typeof(PaperQuestion), paperQuestionId);
        }
        return question;
    }

    internal PaperQuestionRule AddRule(Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score)
    {
        PaperQuestionRule rule = new(ruleId, Id, questionBankId, questionType, count, score)
        {
            TenantId = TenantId
        };

        PaperQuestionRules.Add(rule);
        return rule;
    }

    internal PaperQuestionRule UpdateRule(Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score)
    {
        PaperQuestionRule rule = GetPaperQuestionRule(ruleId);
        rule.QuestionBankId = questionBankId;
        rule.QuestionType = questionType;
        rule.Count = count;
        rule.ScoreEach = score;
        return rule;
    }

    public void RemoveRules(List<Guid> ruleIds)
    {
        foreach (Guid reuleId in ruleIds)
        {
            RemoveRule(reuleId);
        }
    }

    public void RemoveRule(Guid reuleId)
    {
        PaperQuestionRule? rule = GetPaperQuestionRule(reuleId);
        if (rule != null)
        {
            PaperQuestionRules.Remove(rule);
        }
    }

    private PaperQuestionRule GetPaperQuestionRule(Guid ruleId)
    {
        PaperQuestionRule? rule = PaperQuestionRules.SingleOrDefault(q => q.Id == ruleId);
        if (rule == null)
        {
            throw new EntityNotFoundException(typeof(PaperQuestionRule), ruleId);
        }
        return rule;
    }
}