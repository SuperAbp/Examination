using System;
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

    public Task<PaperRepo> GetAsync(Guid examingId, Guid questionRepositoryId)
    {
        return GetAsync(er => er.ExamingId == examingId
                              && er.QuestionRepositoryId == questionRepositoryId);
    }

    public Task<PaperRepo> FindAsync(Guid examingId, Guid questionRepositoryId)
    {
        return FindAsync(er => er.ExamingId == examingId
                               && er.QuestionRepositoryId == questionRepositoryId);
    }

    public Task DeleteAsync(Guid examingId, Guid questionRepositoryId)
    {
        return DeleteAsync(er => er.ExamingId == examingId && er.QuestionRepositoryId == questionRepositoryId);
    }

    public Task DeleteByExamingIdAsync(Guid examingId)
    {
        return DeleteAsync(er => er.ExamingId == examingId);
    }
}