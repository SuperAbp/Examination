using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.Favorites;

public class FavoriteRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
    : EfCoreRepository<ExamDbContext, Favorite, Guid>(dbContextProvider), IFavoriteRepository
{
    public async Task<List<Favorite>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
        Guid? creatorId = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await queryable
        .WhereIf(creatorId.HasValue, p => p.CreatorId == creatorId.Value)
        .OrderBy(string.IsNullOrWhiteSpace(sorting) ? QuestionConsts.DefaultSorting : sorting)
        .PageBy(skipCount, maxResultCount)
        .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<int> CountAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.CreatorId == creatorId)
            .CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid creatorId, Guid questionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(r => r.CreatorId == creatorId, cancellationToken: cancellationToken);
    }
}