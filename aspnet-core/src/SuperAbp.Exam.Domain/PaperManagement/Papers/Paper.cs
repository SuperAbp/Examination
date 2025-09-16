using System;
using System.Diagnostics.CodeAnalysis;
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
        Name = String.Empty;
    }

    [SetsRequiredMembers]
    protected internal Paper(Guid id, string name, decimal score) : base(id)
    {
        Name = name;
        Score = score;
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
    public int TotalQuestionCount { get; set; } = 0;

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    public Guid? TenantId { get; set; }
}