using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.QuestionCategories;

/// <summary>
/// 题目类别
/// </summary>
public class QuestionCategory : FullAuditedEntity<Guid>
{
    protected QuestionCategory()
    {
        Name = String.Empty;
    }

    [SetsRequiredMembers]
    public QuestionCategory(Guid id, string name, Guid? parentId = null)
        : base(id)
    {
        ParentId = parentId;
        Name = name;
    }

    public Guid? ParentId { get; set; }

    public string Name { get; set; }
}