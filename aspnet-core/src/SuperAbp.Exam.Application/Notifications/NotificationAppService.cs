using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Admin.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace SuperAbp.Exam.Notifications;

[Authorize]
public class NotificationAppService(INotificationRepository notificationRepository) : ApplicationService, INotificationAppService
{
    protected INotificationRepository NotificationRepository { get; } = notificationRepository;

    public virtual async Task<long> GetUnreadCountAsync()
    {
        return await NotificationRepository.GetCountAsync(
            CurrentUser.GetId(),
            isRead: false
        );
    }

    public virtual async Task<PagedResultDto<NotificationListDto>> GetListAsync(GetNotificationsInput input)
    {
        var count = await NotificationRepository.GetCountAsync(
            CurrentUser.GetId(),
            isRead: input.IsRead
        );

        var list = await NotificationRepository.GetListAsync(
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            receiverId: CurrentUser.GetId(),
            isRead: input.IsRead
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