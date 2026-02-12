using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Notifications;

public class GetMyNotificationsInput : PagedAndSortedResultRequestDto
{
    /// <summary>
    /// 是否已读
    /// </summary>
    public bool? IsRead { get; set; }
}