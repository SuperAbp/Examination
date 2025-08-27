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

public class FavoriteRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, Favorite, Guid>(dbContextProvider), IFavoriteRepository
{
    public async Task<List<FavoriteWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
        Guid? creatorId = null, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync(creatorId, questionContent, questionType))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FavoriteConsts.DefaultSorting : sorting)
            .PageBy(skipCount, maxResultCount);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<long> CountAsync(Guid? creatorId, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(creatorId, questionContent, questionType))
            .LongCountAsync(cancellationToken);
    }

    private async Task<IQueryable<FavoriteWithDetails>> GetQueryableAsync(Guid? creatorId, string? questionContent = null, QuestionType? questionType = null)
    {
        var dbContext = await GetDbContextAsync();

        IQueryable<Favorite> queryable = (await GetQueryableAsync())
            .WhereIf(creatorId.HasValue, p => p.CreatorId == creatorId.Value);
        IQueryable<Question> questionQueryable = dbContext.Set<Question>().AsQueryable();
        return (from favorite in queryable
                join question in questionQueryable on favorite.QuestionId equals question.Id
                select new FavoriteWithDetails()
                {
                    Id = favorite.Id,
                    QuestionId = question.Id,
                    QuestionContent = question.Content,
                    QuestionType = question.QuestionType,
                    CreationTime = favorite.CreationTime
                })
                .WhereIf(!questionContent.IsNullOrWhiteSpace(), f => f.QuestionContent.Contains(questionContent))
                .WhereIf(questionType is not null, f => f.QuestionType == questionType);
    }

    public async Task<bool> ExistsAsync(Guid creatorId, Guid questionId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .AnyAsync(r => r.CreatorId == creatorId && r.QuestionId == questionId, cancellationToken: cancellationToken);
    }

    public async Task DeleteByQuestionIdAsync(Guid questionId)
    {
        await DeleteAsync(f => f.QuestionId == questionId);
    }
}