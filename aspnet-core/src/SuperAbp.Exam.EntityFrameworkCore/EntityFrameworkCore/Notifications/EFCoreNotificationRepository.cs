using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.Notifications;

public class EFCoreNotificationRepository : EfCoreRepository<ExamDbContext, Notification, Guid>, INotificationRepository
{
    public EFCoreNotificationRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<Notification>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? receiverId = null,
        NotificationType? type = null,
        bool? isRead = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .WhereIf(receiverId.HasValue, x => x.ReceiverId == receiverId.Value)
            .WhereIf(type is not null, x => x.Type == type)
            .WhereIf(isRead.HasValue, x => x.IsRead == isRead.Value)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Data != null && x.Data.Contains(filter))
            .OrderBy(sorting ?? NotificationConsts.DefaultSorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        Guid? receiverId,
        NotificationType? type = null,
        bool? isRead = null,
        string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .WhereIf(receiverId.HasValue, x => x.ReceiverId == receiverId.Value)
            .WhereIf(type is not null, x => x.Type == type)
            .WhereIf(isRead.HasValue, x => x.IsRead == isRead.Value)
            .WhereIf(!filter.IsNullOrWhiteSpace(), x => x.Data != null && x.Data.Contains(filter))
            .LongCountAsync(cancellationToken);
    }
}