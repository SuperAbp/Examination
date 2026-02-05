using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.Announcements;

public class AnnouncementRepository : EfCoreRepository<ExamDbContext, Announcement, Guid>, IAnnouncementRepository
{
    public AnnouncementRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<Announcement>> GetEffectiveListAsync(DateTime now, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Include(x => x.Category)
            .Where(x => x.IsPublished)
            .Where(x => x.PublishTime.HasValue && x.PublishTime.Value <= now)
            .Where(x => !x.ExpirationTime.HasValue || x.ExpirationTime.Value > now)
            .OrderBy(x => x.Sort)
            .ThenByDescending(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<Announcement>> GetEffectiveListByCategoryIdAsync(Guid categoryId, DateTime now, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();

        return await dbSet
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .Where(x => x.IsPublished)
            .Where(x => x.PublishTime.HasValue && x.PublishTime.Value <= now)
            .Where(x => !x.ExpirationTime.HasValue || x.ExpirationTime.Value > now)
            .OrderBy(x => x.Sort)
            .ThenByDescending(x => x.CreationTime)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<int> GetCountAsync(
        string? title = null,
        Guid? categoryId = null,
        bool? isPublished = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title.Contains(title))
            .WhereIf(categoryId.HasValue, x => x.CategoryId == categoryId.Value)
            .WhereIf(isPublished.HasValue, x => x.IsPublished == isPublished.Value)
            .CountAsync(cancellationToken);
    }

    public virtual async Task<List<Announcement>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string? title = null,
        Guid? categoryId = null,
        bool? isPublished = null,
        CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Include(x => x.Category)
            .WhereIf(!title.IsNullOrWhiteSpace(), x => x.Title.Contains(title))
            .WhereIf(categoryId.HasValue, x => x.CategoryId == categoryId.Value)
            .WhereIf(isPublished.HasValue, x => x.IsPublished == isPublished.Value)
            .OrderBy(sorting.IsNullOrWhiteSpace() ? AnnouncementConsts.DefaultSorting : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }
}