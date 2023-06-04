using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task<PaperRepo> FindAsync(Guid paperId, Guid questionRepositoryId)
    {
        return await FindAsync(er => er.PaperId == paperId
                                     && er.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task<List<PaperRepo>> GetListAsync(Guid paperId)
    {
        return await GetListAsync(r => r.PaperId == paperId);
    }

    public async Task DeleteAsync(Guid paperId, Guid questionRepositoryId)
    {
        await DeleteAsync(er => er.PaperId == paperId && er.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task DeleteByExamingIdAsync(Guid paperId)
    {
        await DeleteAsync(er => er.PaperId == paperId);
    }
}