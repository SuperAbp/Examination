using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.Questions;

/// <summary>
/// 问题
/// </summary>
public class QuestionRepository : EfCoreRepository<ExamDbContext, Question, Guid>, IQuestionRepository
{
    /// <summary>
    /// .ctor
    ///</summary>
    public QuestionRepository(
        IDbContextProvider<ExamDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<int> GetCountAsync(Guid questionRepositoryId)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId)
            .CountAsync();
    }

    public async Task<int> GetCountAsync(Guid questionRepositoryId, QuestionType questionType)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId && r.QuestionType == questionType)
            .CountAsync();
    }

    public async Task<List<QuestionType>> GetQuestionTypesAsync(Guid questionRepositoryId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(q => q.QuestionRepositoryId == questionRepositoryId)
            .GroupBy(q => q.QuestionType)
            .Select(q => q.Key)
            .ToListAsync();
    }

    public async Task<List<Question>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? questionRepositoryId = null,
        QuestionType? questionType = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        return await queryable
            .WhereIf(questionRepositoryId.HasValue, p => p.QuestionRepositoryId == questionRepositoryId.Value)
            .WhereIf(questionType.HasValue, p => p.QuestionType == questionType.Value)
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? QuestionConsts.DefaultSorting : sorting)
            .OrderBy(q => Guid.NewGuid())
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<Question>> GetRandomListAsync(int maxResultCount = Int32.MaxValue, Guid? questionRepositoryId = null,
        QuestionType? questionType = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        return await queryable
            .WhereIf(questionRepositoryId.HasValue, p => p.QuestionRepositoryId == questionRepositoryId.Value)
            .WhereIf(questionType.HasValue, p => p.QuestionType == questionType.Value)
            .OrderBy(q => Guid.NewGuid())
            .Take(maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> AnyAsync(Guid questionRepositoryId, Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.AnyAsync(q => q.QuestionRepositoryId == questionRepositoryId && q.Id == questionId, GetCancellationToken(cancellationToken));
    }

    public async Task<bool> ContentExistsAsync(string content, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Content == content, GetCancellationToken(cancellationToken));
    }
}