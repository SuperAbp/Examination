using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

/// <summary>
/// 管理员获取通知列表输入参数
/// </summary>
public class GetNotificationsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }
}