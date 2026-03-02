using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Notifications;
using SuperAbp.Exam.Notifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Controllers;

[Route("api/notifications")]
public class NotificationController : AbpController, INotificationAppService
{
    private readonly INotificationAppService _notificationAppService;

    public NotificationController(INotificationAppService notificationAppService)
    {
        _notificationAppService = notificationAppService;
    }

    [HttpGet("unread-count")]
    public virtual async Task<long> GetUnreadCountAsync()
    {
        return await _notificationAppService.GetUnreadCountAsync();
    }

    [HttpGet]
    public virtual async Task<PagedResultDto<NotificationListDto>> GetListAsync(GetNotificationsInput input)
    {
        return await _notificationAppService.GetListAsync(input);
    }

    [HttpPost("{id}/mark-as-read")]
    public virtual async Task MarkAsReadAsync(Guid id)
    {
        await _notificationAppService.MarkAsReadAsync(id);
    }

    [HttpPost("mark-all-as-read")]
    public virtual async Task MarkAllAsReadAsync()
    {
        await _notificationAppService.MarkAllAsReadAsync();
    }
}