using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Timing;
using SuperAbp.Exam.Notifications;

namespace SuperAbp.Exam.Admin.Notifications;

[Authorize]
public class NotificationAdminAppService(INotificationRepository notificationRepository, INotificationPublisher notificationPublisher)
    : ExamAppService, INotificationAdminAppService
{
    protected INotificationRepository NotificationRepository { get; } = notificationRepository;
    protected INotificationPublisher NotificationPublisher { get; } = notificationPublisher;

    public virtual async Task<long> GetUnreadCountAsync()
    {
        return await NotificationRepository.GetCountAsync(
            CurrentUser.GetId(),
            isRead: false
        );
    }

    public virtual async Task<PagedResultDto<NotificationMyListDto>> GetMyListAsync(GetMyNotificationsInput input)
    {
        var count = await NotificationRepository.GetCountAsync(
            CurrentUser.GetId(),
            isRead: input.IsRead,
            filter: input.Filter
        );

        var list = await NotificationRepository.GetListAsync(
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            CurrentUser.GetId(),
            isRead: input.IsRead,
            filter: input.Filter
        );

        return new PagedResultDto<NotificationMyListDto>(
            count,
            ObjectMapper.Map<List<Notification>, List<NotificationMyListDto>>(list)
        );
    }

    [Authorize(ExamPermissions.Notifications.Management)]
    public virtual async Task<PagedResultDto<NotificationListDto>> GetListAsync(GetNotificationsInput input)
    {
        var count = await NotificationRepository.GetCountAsync(
            input.ReceiverId,
            input.Type.HasValue ? NotificationType.FromValue(input.Type.Value) : null,
            input.IsRead
        );

        var list = await NotificationRepository.GetListAsync(
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            input.ReceiverId,
            input.Type.HasValue ? NotificationType.FromValue(input.Type.Value) : null,
            input.IsRead
        );

        return new PagedResultDto<NotificationListDto>(
            count,
            ObjectMapper.Map<List<Notification>, List<NotificationListDto>>(list)
        );
    }

    public virtual async Task MarkAsReadAsync(Guid id)
    {
        var notification = await NotificationRepository.GetAsync(id);
        if (notification.ReceiverId != CurrentUser.GetId())
        {
            throw new BusinessException(ExamDomainErrorCodes.Notifications.NotFound);
        }
        notification.MarkAsRead(Clock.Now);
        await NotificationRepository.UpdateAsync(notification);
    }

    public virtual async Task MarkAllAsReadAsync()
    {
        var unread = await NotificationRepository.GetListAsync(
            receiverId: CurrentUser.GetId(),
            isRead: false
        );
        foreach (var notification in unread)
        {
            notification.MarkAsRead(Clock.Now);
        }
        await NotificationRepository.UpdateManyAsync(unread);
    }
}