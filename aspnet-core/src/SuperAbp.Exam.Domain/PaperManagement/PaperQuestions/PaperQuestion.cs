using SuperAbp.Exam.PaperManagement.PaperSections;
using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.PaperManagement.PaperQuestions;

/// <summary>
/// 试卷题目
/// </summary>
public class PaperQuestion : Entity<Guid>, IHasCreationTime, ISoftDelete, IMultiTenant
{
    protected PaperQuestion()
    {
    }

    public PaperQuestion(Guid id, Guid paperSectionId, Guid questionId)
        : base(id)
    {
        PaperSectionId = paperSectionId;
        QuestionId = questionId;
    }

    public Guid PaperSectionId { get; set; }
    public Guid QuestionId { get; set; }
    public int Order { get; set; }
    public decimal Score { get; set; }

    public PaperSection PaperSection { get; set; }
    public DateTime CreationTime { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? TenantId { get; set; }
}