using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.MistakesReviews;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.MistakesReviews;

public class MistakesReviewRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, MistakesReview, Guid>(dbContextProvider), IMistakesReviewRepository
{
    public async Task<List<MistakeWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
        Guid? userId = null, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default)
    {
        var query = (await GetQueryableAsync(userId, questionContent, questionType))
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? FavoriteConsts.DefaultSorting : sorting)
            .Skip(skipCount)
            .Take(maxResultCount);
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<long> CountAsync(Guid? userId, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync(userId, questionContent, questionType))
            .LongCountAsync(cancellationToken);
    }

    private async Task<IQueryable<MistakeWithDetails>> GetQueryableAsync(Guid? userId, string? questionContent = null, QuestionType? questionType = null)
    {
        var dbContext = await GetDbContextAsync();
        IQueryable<MistakesReview> queryable = (await GetQueryableAsync())
            .WhereIf(userId.HasValue, p => p.UserId == userId.Value);
        IQueryable<Question> questionQueryable = dbContext.Set<Question>().AsQueryable();
        return (from mistake in queryable
                join question in questionQueryable on mistake.QuestionId equals question.Id
                select new MistakeWithDetails()
                {
                    Id = mistake.Id,
                    QuestionId = question.Id,
                    QuestionContent = question.Content,
                    QuestionType = question.QuestionType,
                    CreationTime = mistake.CreationTime,
                    LastModificationTime = mistake.CreationTime // 可扩展为最后一次错误时间
                })
                .WhereIf(!string.IsNullOrWhiteSpace(questionContent), f => f.QuestionContent.Contains(questionContent))
                .WhereIf(questionType is not null, f => f.QuestionType == questionType);
    }
}