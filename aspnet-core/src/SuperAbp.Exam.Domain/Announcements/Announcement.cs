using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告
/// </summary>
public class Announcement : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected Announcement()
    { }

    [SetsRequiredMembers]
    public Announcement(Guid id, string title, string content, int sort = 0, Guid? categoryId = null)
        : base(id)
    {
        Title = title;
        Content = content;
        Sort = sort;
        CategoryId = categoryId;
        IsPublished = false;
    }

    public Guid? TenantId { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 预定发布时间
    /// </summary>
    public DateTime? ScheduledPublishTime { get; set; }

    /// <summary>
    /// 预定到期时间
    /// </summary>
    public DateTime? ScheduledExpirationTime { get; set; }

    /// <summary>
    /// 是否发布
    /// </summary>
    public bool IsPublished { get; set; }

    /// <summary>
    /// 显示顺序（越小越靠前）
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 公告分类
    /// </summary>
    public virtual AnnouncementCategory? Category { get; set; }

    /// <summary>
    /// 发布
    /// </summary>
    public void Publish()
    {
        if (IsPublished)
        {
            return;
        }

        IsPublished = true;
    }

    /// <summary>
    /// 设置发布时间
    /// </summary>
    public void SetPublishTime(DateTime publishTime)
    {
        if (IsPublished)
        {
            return;
        }
        ScheduledPublishTime = publishTime;
    }

    /// <summary>
    /// 下架
    /// </summary>
    public void Unpublish()
    {
        IsPublished = false;
        ScheduledPublishTime = null;
        ScheduledExpirationTime = null;
    }

    /// <summary>
    /// 检查是否在有效期内
    /// </summary>
    public bool IsEffective(DateTime now)
    {
        if (!IsPublished)
        {
            return false;
        }

        // 如果设置了发布时间，需要检查是否已到发布时间
        if (ScheduledPublishTime.HasValue && ScheduledPublishTime.Value > now)
        {
            return false;
        }

        // 检查是否已过期
        if (ScheduledExpirationTime.HasValue && now > ScheduledExpirationTime.Value)
        {
            return false;
        }

        return true;
    }
}