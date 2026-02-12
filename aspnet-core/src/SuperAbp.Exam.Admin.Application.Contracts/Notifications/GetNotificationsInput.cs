using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

/// <summary>
/// 通知列表
/// </summary>
public class GetNotificationsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 接收用户ID筛选
    /// </summary>
    public Guid? ReceiverId { get; set; }

    /// <summary>
    /// 通知类型筛选
    /// </summary>
    public int? Type { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// 发送渠道筛选
    /// </summary>
    public int? Channel { get; set; }
}