using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

public class NotificationMyListDto : EntityDto<Guid>
{
    /// <summary>
    /// 通知类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 数据(JSON格式)
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 阅读时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 关联实体ID
    /// </summary>
    public Guid? RelatedEntityId { get; set; }

    /// <summary>
    /// 关联实体类型
    /// </summary>
    public string RelatedEntityType { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }
}