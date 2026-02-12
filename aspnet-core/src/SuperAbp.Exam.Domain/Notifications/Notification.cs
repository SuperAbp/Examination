using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知记录
/// </summary>
public class Notification : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    protected Notification()
    {
    }

    [SetsRequiredMembers]
    public Notification(
        Guid id,
        Guid receiverId,
        NotificationType type,
        [CanBeNull] string? data = null,
        [CanBeNull] Guid? relatedEntityId = null,
        [CanBeNull] string? relatedEntityType = null)
        : base(id)
    {
        ReceiverId = receiverId;
        Type = type;
        Data = data ?? string.Empty;
        IsRead = false;
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
    }

    /// <summary>
    /// 租户Id
    /// </summary>
    public Guid? TenantId { get; protected set; }

    /// <summary>
    /// 接收用户ID
    /// </summary>
    public Guid ReceiverId { get; protected set; }

    /// <summary>
    /// 通知类型
    /// </summary>
    public NotificationType Type { get; protected set; }

    /// <summary>
    /// 数据(JSON格式，前端渲染用)
    /// </summary>
    public string? Data { get; protected set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; protected set; }

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime? ReadTime { get; protected set; }

    /// <summary>
    /// 关联实体ID
    /// </summary>
    public Guid? RelatedEntityId { get; protected set; }

    /// <summary>
    /// 关联实体类型
    /// </summary>
    public string? RelatedEntityType { get; protected set; }

    /// <summary>
    /// 标记为已读
    /// </summary>
    public virtual void MarkAsRead(DateTime readTime)
    {
        if (!IsRead)
        {
            IsRead = true;
            ReadTime = readTime;
        }
    }

    /// <summary>
    /// 设置关联实体
    /// </summary>
    public virtual void SetRelatedEntity([CanBeNull] Guid? relatedEntityId, [CanBeNull] string relatedEntityType)
    {
        RelatedEntityId = relatedEntityId;
        RelatedEntityType = relatedEntityType;
    }
}