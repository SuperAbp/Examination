using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Announcements;

/// <summary>
/// 公告
/// </summary>
public class AnnouncementListDto : FullAuditedEntityDto<Guid>
{
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
    /// 显示顺序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string? CategoryName { get; set; }
}