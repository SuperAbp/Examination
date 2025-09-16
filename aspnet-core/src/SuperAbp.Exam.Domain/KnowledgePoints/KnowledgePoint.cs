using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.KnowledgePoints;

/// <summary>
/// 知识点
/// </summary>
public class KnowledgePoint : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected KnowledgePoint()
    {
        Name = String.Empty;
    }

    [SetsRequiredMembers]
    public KnowledgePoint(Guid id, string name, Guid? parentId = null)
        : base(id)
    {
        ParentId = parentId;
        Name = name;
    }

    public Guid? ParentId { get; set; }

    public string Name { get; set; }
    public Guid? TenantId { get; set; }
}