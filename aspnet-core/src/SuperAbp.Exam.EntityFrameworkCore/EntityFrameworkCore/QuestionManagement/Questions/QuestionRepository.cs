using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.Questions;

public class QuestionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, Question, Guid>(dbContextProvider), IQuestionRepository
{
    public async Task<bool> ExistsQuestionTypeAsync(int questionType, List<Guid> ids, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.AnyAsync(q => q.QuestionType == questionType && ids.Contains(q.Id), cancellationToken);
    }

    public async Task<int> GetCountAsync(Guid questionBankId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionBankId == questionBankId)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(Guid questionBankId, QuestionType questionType, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionBankId == questionBankId && r.QuestionType == questionType)
            .CountAsync(cancellationToken);
    }

    public async Task<List<QuestionType>> GetQuestionTypesAsync(Guid questionBankId, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(q => q.QuestionBankId == questionBankId)
            .GroupBy(q => q.QuestionType)
            .Select(q => q.Key)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(string? content = null,
        int? questionType = null,
        List<Guid>? questionBankIds = null,
        List<Guid>? excludeIds = null,
        CancellationToken cancellationToken = default)
    {
        var questionQueryable = await GetQueryableAsync(content, questionType, questionBankIds, excludeIds);
        return await questionQueryable.CountAsync(cancellationToken);
    }

    public async Task<List<Question>> GetByIdsAsync(IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        return await GetListAsync(q => ids.Contains(q.Id), cancellationToken: cancellationToken, includeDetails: true);
    }

    public async Task<List<QuestionWithDetails>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string? content = null,
        int? questionType = null,
        List<Guid>? questionBankIds = null,
        List<Guid>? includeIds = null,
        List<Guid>? excludeIds = null,
        bool? includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var questionQueryable = await GetQueryableAsync(content, questionType, questionBankIds, excludeIds);
        questionQueryable = questionQueryable
            .IncludeIf(includeDetails.HasValue && includeDetails.Value, q => q.Options)
            .WhereIf(includeIds?.Count > 0, q => includeIds.Contains(q.Id));
        var questionBankQueryable = dbContext.Set<QuestionBank>().AsQueryable();
        var questionKnowledgePointQueryable = dbContext.Set<QuestionKnowledgePoint>().AsQueryable();
        var knowledgePointQueryable = dbContext.Set<KnowledgePoint>().AsQueryable();

        var pointQueryable = from qkp in questionKnowledgePointQueryable
                             join kp in knowledgePointQueryable on qkp.KnowledgePointId equals kp.Id
                             select new { qkp.QuestionId, kp.Name };

        var queryable = (from q in questionQueryable
                         join qb in questionBankQueryable on q.QuestionBankId equals qb.Id
                         join kp in pointQueryable on q.Id equals kp.QuestionId into kpGroup
                         select new QuestionWithDetails
                         {
                             Id = q.Id,
                             QuestionBank = qb.Title,
                             Content = q.Content,
                             Analysis = q.Analysis,
                             QuestionType = q.QuestionType,
                             CreationTime = q.CreationTime,
                             KnowledgePoints = kpGroup.Select(k => k.Name).ToList(),
                             Options = q.Options
                         })
                        .PageBy(skipCount, maxResultCount);

        return await queryable.ToListAsync(cancellationToken);
    }

    private async Task<IQueryable<Question>> GetQueryableAsync(string? content, int? questionType, List<Guid>? questionBankIds, List<Guid>? excludeIds = null)
    {
        return (await GetQueryableAsync())
            .WhereIf(questionBankIds is not null && questionBankIds.Count > 0, q => questionBankIds.Contains(q.QuestionBankId))
            .WhereIf(questionType.HasValue, q => q.QuestionType == questionType.Value)
            .WhereIf(!content.IsNullOrWhiteSpace(), q => q.Content.Contains(content))
            .WhereIf(excludeIds?.Count > 0, q => !excludeIds.Contains(q.Id));
    }

    public async Task<List<Question>> GetRandomListAsync(int maxResultCount = Int32.MaxValue, Guid? questionRepositoryId = null,
        int? questionType = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Question> queryable = (await GetQueryableAsync())
            .WhereIf(questionRepositoryId.HasValue, p => p.QuestionBankId == questionRepositoryId.Value)
            .WhereIf(questionType.HasValue, p => p.QuestionType == questionType.Value);
        var dbContext = await GetDbContextAsync();
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
        return await dbSet.AnyAsync(q => q.QuestionBankId == questionRepositoryId && q.Id == questionId, cancellationToken);
    }

    public async Task<bool> ContentExistsAsync(string content, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(x => x.Content == content, cancellationToken);
    }
}