using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.PaperRepos;

/// <summary>
/// 考试题库
/// </summary>
public class PaperRepoRepository : EfCoreRepository<ExamDbContext, PaperRepo, Guid>, IPaperRepoRepository
{
    /// <summary>
    /// .ctor
    ///</summary>
    public PaperRepoRepository(
        IDbContextProvider<ExamDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<PaperRepo> GetAsync(Guid paperId, Guid questionRepositoryId)
    {
        return await GetAsync(er => er.PaperId == paperId
                              && er.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task<PaperRepo?> FindAsync(Guid paperId, Guid questionRepositoryId)
    {
        return await FindAsync(er => er.PaperId == paperId
                                     && er.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task<List<PaperRepo>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? paperId = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        return await queryable
             .WhereIf(paperId.HasValue, p => p.PaperId == paperId.Value)
             .OrderBy(string.IsNullOrWhiteSpace(sorting) ? PaperRepoConsts.DefaultSorting : sorting)
             .PageBy(skipCount, maxResultCount)
             .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid paperId, Guid questionRepositoryId)
    {
        await DeleteAsync(er => er.PaperId == paperId && er.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task DeleteByExamIdAsync(Guid paperId)
    {
        await DeleteAsync(er => er.PaperId == paperId);
    }
}