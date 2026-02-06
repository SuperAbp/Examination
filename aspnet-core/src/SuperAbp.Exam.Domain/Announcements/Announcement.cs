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
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpirationTime { get; set; }

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
        PublishTime = publishTime;
    }

    /// <summary>
    /// 下架
    /// </summary>
    public void Unpublish()
    {
        IsPublished = false;
    }

    /// <summary>
    /// 检查是否在有效期内
    /// </summary>
    public bool IsEffective(DateTime now)
    {
        if (!IsPublished || !PublishTime.HasValue)
        {
            return false;
        }

        if (ExpirationTime.HasValue && now > ExpirationTime.Value)
        {
            return false;
        }

        if (PublishTime.Value > now)
        {
            return false;
        }

        return true;
    }
}