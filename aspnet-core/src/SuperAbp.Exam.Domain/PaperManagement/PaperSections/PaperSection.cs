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
    public PaperSection(Guid id, Guid paperId, string title, decimal scoreEach, int order)
        : base(id)
    {
        Id = id;
        PaperId = paperId;
        Title = title;
        ScoreEach = scoreEach;
        TotalScore = 0;
        Order = order;
        TotalCount = 0;
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
        RecalculateTotals();
        return this;
    }

    internal PaperSection UpdateQuestion(Guid paperQuestionId, decimal score, int order)
    {
        PaperQuestion question = GetPaperQuestion(paperQuestionId);
        question.Score = score;
        question.Order = order;
        RecalculateTotals();
        return this;
    }

    public void RemoveQuestions(List<Guid> questionIds)
    {
        foreach (Guid questionId in questionIds)
        {
            RemoveQuestion(questionId);
        }
        RecalculateTotals();
    }

    public void RemoveQuestion(Guid paperQuestionId)
    {
        PaperQuestion question = GetPaperQuestion(paperQuestionId);
        PaperQuestions.Remove(question);
        RecalculateTotals();
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
        RecalculateTotals();
        return rule;
    }

    internal PaperQuestionRule UpdateRule(Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score)
    {
        PaperQuestionRule rule = GetPaperQuestionRule(ruleId);
        rule.QuestionBankId = questionBankId;
        rule.QuestionType = questionType;
        rule.Count = count;
        rule.Score = score;
        RecalculateTotals();
        return rule;
    }

    public void RemoveRules(List<Guid> ruleIds)
    {
        foreach (Guid reuleId in ruleIds)
        {
            RemoveRule(reuleId);
        }
        RecalculateTotals();
    }

    public void RemoveRule(Guid reuleId)
    {
        PaperQuestionRule rule = GetPaperQuestionRule(reuleId);

        PaperQuestionRules.Remove(rule);
        RecalculateTotals();
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

    private void RecalculateTotals()
    {
        TotalScore = PaperQuestions.Sum(q => q.Score) + PaperQuestionRules.Sum(r => r.Score);
        TotalCount = PaperQuestions.Count + PaperQuestionRules.Sum(r => r.Count);
    }
}