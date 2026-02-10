using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Announcements;

/// <summary>
/// 公告（用户端）
/// </summary>
public class AnnouncementListDto : EntityDto<Guid>
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 简短内容摘要
    /// </summary>
    public string BriefContent { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// 分类名称
    /// </summary>
    public string? CategoryName { get; set; }
}