using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.AuditLogging;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告分类
/// </summary>
public class AnnouncementCategory : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected AnnouncementCategory()
    {
    }

    [SetsRequiredMembers]
    public AnnouncementCategory(Guid id, string name, int sort = 0, string? remark = null)
        : base(id)
    {
        Name = name;
        Sort = sort;
        Remark = remark;
        Announcements = new List<Announcement>();
    }

    public Guid? TenantId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 显示顺序（越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 公告列表
    /// </summary>
    public virtual ICollection<Announcement> Announcements { get; set; }
}