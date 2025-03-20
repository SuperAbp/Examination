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
using Polly;

namespace SuperAbp.Exam.EntityFrameworkCore.Favorites;

public class FavoriteRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
    : EfCoreRepository<ExamDbContext, Favorite, Guid>(dbContextProvider), IFavoriteRepository
{
    public async Task<List<FavoriteWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
        Guid? creatorId = null, CancellationToken cancellationToken = default)
    {
        ExamDbContext dbContext = await GetDbContextAsync();

        IQueryable<Favorite> queryable = (await GetQueryableAsync()).WhereIf(creatorId.HasValue, p => p.CreatorId == creatorId.Value);
        IQueryable<Question> questionQueryable = dbContext.Set<Question>().AsQueryable();
        var query = (from favorite in queryable
                     join question in questionQueryable on favorite.QuestionId equals question.Id
                     select new FavoriteWithDetails()
                     {
                         Id = favorite.Id,
                         QuestionName = question.Content,
                         CreationTime = favorite.CreationTime
                     })
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? QuestionConsts.DefaultSorting : sorting)
            .PageBy(skipCount, maxResultCount);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<long> CountAsync(Guid creatorId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.CreatorId == creatorId)
            .LongCountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid creatorId, Guid questionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(r => r.CreatorId == creatorId, cancellationToken: cancellationToken);
    }
}