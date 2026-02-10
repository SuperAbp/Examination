using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告详情（用户端）
/// </summary>
public class AnnouncementDetailDto : CreationAuditedEntityDto<Guid>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 完整内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string? CategoryName { get; set; }
}