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

public class QuestionRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
    : EfCoreRepository<ExamDbContext, Question, Guid>(dbContextProvider), IQuestionRepository
{
    public async Task<int> GetCountAsync(Guid questionRepositoryId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(Guid questionRepositoryId, QuestionType questionType, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId && r.QuestionType == questionType)
            .CountAsync(cancellationToken);
    }

    public async Task<List<QuestionType>> GetQuestionTypesAsync(Guid questionRepositoryId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(q => q.QuestionRepositoryId == questionRepositoryId)
            .GroupBy(q => q.QuestionType)
            .Select(q => q.Key)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Question>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? questionRepositoryId = null,
        int? questionType = null,
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
        int? questionType = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Question> queryable = (await GetQueryableAsync())
            .WhereIf(questionRepositoryId.HasValue, p => p.QuestionRepositoryId == questionRepositoryId.Value)
            .WhereIf(questionType.HasValue, p => p.QuestionType == questionType.Value);
        ExamDbContext dbContext = await GetDbContextAsync();
        if (dbContext.Database.ProviderName?.ToLower().Contains("sqlserver") ?? false)
        {
            return await queryable
                .OrderBy(q => Guid.NewGuid())
                .Take(maxResultCount)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        return await queryable
            .OrderBy(q => EF.Functions.Random())
            .Take(maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> AnyAsync(Guid questionRepositoryId, Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.AnyAsync(q => q.QuestionRepositoryId == questionRepositoryId && q.Id == questionId, cancellationToken);
    }

    public async Task<bool> ContentExistsAsync(string content, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Content == content, cancellationToken);
    }
}