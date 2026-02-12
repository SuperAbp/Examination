using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Notifications;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace SuperAbp.Exam.Admin.Controllers;

[Route("api/notifications")]
public class NotificationController(INotificationAppService notificationAppService) : AbpController, INotificationAppService
{
    protected INotificationAppService NotificationAppService { get; } = notificationAppService;

    [HttpGet("unread-count")]
    public async Task<long> GetUnreadCountAsync()
    {
        return await NotificationAppService.GetUnreadCountAsync();
    }

    [HttpGet]
    public virtual async Task<PagedResultDto<NotificationListDto>> GetListAsync(GetNotificationsInput input)
    {
        return await NotificationAppService.GetListAsync(input);
    }

    [HttpGet("my")]
    public virtual async Task<PagedResultDto<NotificationMyListDto>> GetMyListAsync(GetMyNotificationsInput input)
    {
        return await NotificationAppService.GetMyListAsync(input);
    }

    [HttpPost("{id}/mark-as-read")]
    public virtual async Task MarkAsReadAsync(Guid id)
    {
        await NotificationAppService.MarkAsReadAsync(id);
    }

    [HttpPost("mark-all-as-read")]
    public virtual async Task MarkAllAsReadAsync()
    {
        await NotificationAppService.MarkAllAsReadAsync();
    }
}