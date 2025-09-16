using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks;

/// <summary>
/// 题库
/// </summary>
public class QuestionBank : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected QuestionBank()
    { Title = String.Empty; }

    [SetsRequiredMembers]
    protected internal QuestionBank(Guid id, string title) : base(id)
    {
        Title = title;
    }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; internal set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    public Guid? TenantId { get; set; }
}