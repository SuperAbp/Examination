using SuperAbp.Exam.PaperManagement.PaperSections;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.PaperManagement.PaperQuestionRules;

/// <summary>
/// 抽题规则
/// </summary>
public class PaperQuestionRule : Entity<Guid>, IHasCreationTime, ISoftDelete, IMultiTenant
{
    protected PaperQuestionRule()
    { }

    public PaperQuestionRule(Guid id, Guid paperSectionId, Guid questionBankId, QuestionType questionType, int count, decimal score) : base(id)
    {
        PaperSectionId = paperSectionId;
        QuestionBankId = questionBankId;
        QuestionType = questionType;
        Count = count;
        Score = score;
    }

    /// <summary>
    /// 大题Id
    /// </summary>
    public Guid PaperSectionId { get; set; }

    /// <summary>
    /// 题库Id
    /// </summary>
    public Guid QuestionBankId { get; set; }

    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    public PaperSection PaperSection { get; set; }

    public DateTime CreationTime { get; set; }

    public bool IsDeleted { get; set; }
    public Guid? TenantId { get; set; }
}