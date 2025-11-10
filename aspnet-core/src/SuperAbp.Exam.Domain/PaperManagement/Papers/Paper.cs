using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.PaperSections;
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
    protected internal Paper(Guid id, PaperType paperType, string name, decimal score, int totalQuestionCount) : base(id)
    {
        Name = name;
        Score = score;
        PaperType = paperType;
        TotalQuestionCount = totalQuestionCount;
        PaperSections = [];
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
    public int TotalQuestionCount { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    public PaperType PaperType { get; set; }

    public List<PaperSection> PaperSections { get; set; }

    public Guid? TenantId { get; set; }

    public Paper AddSection(Guid sectionId, string title, decimal scoreEach, decimal totalScore, int order, int totalCount)
    {
        PaperSection section = new(sectionId, Id, title, scoreEach, totalScore, order, totalCount);
        PaperSections.Add(section);
        return this;
    }

    public Paper UpdateSection(Guid sectionId, string title, decimal scoreEach, decimal totalScore, int order, int totalCount)
    {
        PaperSection section = GetSection(sectionId);
        section.Title = title;
        section.ScoreEach = scoreEach;
        section.TotalScore = totalScore;
        section.Order = order;
        section.TotalCount = totalCount;
        return this;
    }

    public Paper RemoveSection(Guid sectionId)
    {
        PaperSection section = GetSection(sectionId);
        PaperSections.Remove(section);
        return this;
    }

    public Paper RemoveSections(List<Guid> sectionIds)
    {
        foreach (Guid sectionId in sectionIds)
        {
            RemoveSection(sectionId);
        }
        return this;
    }

    public Paper AddQuestion(Guid sectionId, Guid paperQuestionId, Guid questionId, decimal score, int order)
    {
        PaperSection section = GetSection(sectionId);
        section.AddQuestion(paperQuestionId, questionId, score, order);
        return this;
    }

    public Paper UpdateQuestion(Guid sectionId, Guid paperQuestionId, decimal score, int order)
    {
        PaperSection section = GetSection(sectionId);
        section.UpdateQuestion(paperQuestionId, score, order);
        return this;
    }

    public Paper RemoveQuestion(Guid sectionId, Guid questionId)
    {
        PaperSection section = GetSection(sectionId);
        section.RemoveQuestion(questionId);
        return this;
    }

    public Paper AddRule(Guid sectionId, Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score)
    {
        PaperSection section = GetSection(sectionId);
        section.AddRule(ruleId, questionBankId, questionType, count, score);
        return this;
    }

    public Paper UpdateRule(Guid sectionId, Guid ruleId, Guid questionBankId, QuestionType questionType, int count, decimal score)
    {
        PaperSection section = GetSection(sectionId);
        section.UpdateRule(ruleId, questionBankId, questionType, count, score);
        return this;
    }

    public Paper RemoveRule(Guid sectionId, Guid ruleId)
    {
        PaperSection section = GetSection(sectionId);
        section.RemoveRule(ruleId);
        return this;
    }

    public Paper RemoveRules(Guid sectionId, List<Guid> ruleIds)
    {
        PaperSection section = GetSection(sectionId);
        foreach (Guid ruleId in ruleIds)
        {
            section.RemoveRule(ruleId);
        }
        return this;
    }

    private PaperSection GetSection(Guid sectionId)
    {
        return PaperSections.SingleOrDefault(x => x.Id == sectionId)
            ?? throw new EntityNotFoundException(typeof(PaperSection), sectionId);
    }
}