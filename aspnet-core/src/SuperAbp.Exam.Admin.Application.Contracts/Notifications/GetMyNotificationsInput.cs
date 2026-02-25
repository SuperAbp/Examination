using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

public class GetMyNotificationsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// 筛选关键字（标题/内容）
    /// </summary>
    public string? Filter { get; set; }
}