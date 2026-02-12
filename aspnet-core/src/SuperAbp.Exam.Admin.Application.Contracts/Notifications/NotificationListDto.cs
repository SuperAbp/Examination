using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

public class NotificationListDto : EntityDto<Guid>
{
    /// <summary>
    /// 接收用户ID
    /// </summary>
    public Guid ReceiverId { get; set; }

    /// <summary>
    /// 通知类型
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }
}