using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.PaperManagement.Papers;

/// <summary>
/// 试卷
/// </summary>
public class Paper : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected Paper()
    {
    }

    [SetsRequiredMembers]
    protected internal Paper(Guid id, PaperType paperType, string name, bool manualReview) : base(id)
    {
        Name = name;
        PaperType = paperType;
        ManualReview = manualReview;
        PaperSections = [];
        Score = 0;
        TotalQuestionCount = 0;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 总题数
    /// </summary>
    public int TotalQuestionCount { get; private set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; private set; }

    /// <summary>
    /// 人工批阅
    /// </summary>
    public bool ManualReview { get; set; }

    public PaperType PaperType { get; set; }

    public List<PaperSection> PaperSections { get; private set; }

    public Guid? TenantId { get; set; }

    public Paper AddSection(Guid sectionId, string title, decimal scoreEach, int order)
    {
        PaperSection section = new(sectionId, Id, title, scoreEach, order);
        PaperSections.Add(section);
        RecalculateTotals();
        return this;
    }

    public Paper UpdateSection(Guid sectionId, string title, decimal scoreEach, int order)
    {
        PaperSection section = GetSection(sectionId);
        section.Title = title;
        section.ScoreEach = scoreEach;
        section.Order = order;
        RecalculateTotals();
        return this;
    }

    public Paper RemoveSection(Guid sectionId)
    {
        PaperSection section = GetSection(sectionId);
        PaperSections.Remove(section);
        RecalculateTotals();
        return this;
    }

    public Paper RemoveSections(List<Guid> sectionIds)
    {
        foreach (Guid sectionId in sectionIds)
        {
            RemoveSection(sectionId);
        }
        RecalculateTotals();
        return this;
    }

    public Paper AddQuestion(Guid sectionId, Guid paperQuestionId, Guid questionId, decimal score, int order)
    {
        PaperSection section = GetSection(sectionId);
        section.AddQuestion(paperQuestionId, questionId, score, order);
        RecalculateTotals();
        return this;
    }

    public Paper UpdateQuestion(Guid sectionId, Guid paperQuestionId, decimal score, int order)
    {
        PaperSection section = GetSection(sectionId);
        section.UpdateQuestion(paperQuestionId, score, order);
        RecalculateTotals();
        return this;
    }

    public Paper RemoveQuestion(Guid sectionId, Guid questionId)
    {
        PaperSection section = GetSection(sectionId);
        section.RemoveQuestion(questionId);
        RecalculateTotals();
        return this;
    }

    public Paper AddRule(Guid sectionId, Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score, Guid? knowledgePointId = null)
    {
        PaperSection section = GetSection(sectionId);
        section.AddRule(ruleId, questionBankId, questionType, count, score, knowledgePointId);
        RecalculateTotals();
        return this;
    }

    public Paper UpdateRule(Guid sectionId, Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score, Guid? knowledgePointId = null)
    {
        PaperSection section = GetSection(sectionId);
        section.UpdateRule(ruleId, questionBankId, questionType, count, score, knowledgePointId);
        RecalculateTotals();
        return this;
    }

    public Paper RemoveRule(Guid sectionId, Guid ruleId)
    {
        PaperSection section = GetSection(sectionId);
        section.RemoveRule(ruleId);
        RecalculateTotals();
        return this;
    }

    public Paper RemoveRules(Guid sectionId, List<Guid> ruleIds)
    {
        PaperSection section = GetSection(sectionId);
        foreach (Guid ruleId in ruleIds)
        {
            section.RemoveRule(ruleId);
        }
        RecalculateTotals();
        return this;
    }

    private PaperSection GetSection(Guid sectionId)
    {
        return PaperSections.SingleOrDefault(x => x.Id == sectionId)
            ?? throw new EntityNotFoundException(typeof(PaperSection), sectionId);
    }

    private void RecalculateTotals()
    {
        Score = PaperSections.Sum(s => s.TotalScore);
        TotalQuestionCount = PaperSections.Sum(s => s.TotalCount);
    }
}