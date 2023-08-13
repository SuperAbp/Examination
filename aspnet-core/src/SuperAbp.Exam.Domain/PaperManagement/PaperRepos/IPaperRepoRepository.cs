using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperRepos
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public interface IPaperRepoRepository : IRepository<PaperRepo, Guid>
    {
        Task<PaperRepo> GetAsync(Guid paperId, Guid questionRepositoryId);

        Task<PaperRepo> FindAsync(Guid paperId, Guid questionRepositoryId);

        Task<List<PaperRepo>> GetListAsync(Guid paperId);

        Task DeleteAsync(Guid paperId, Guid questionRepositoryId);

        Task DeleteByExamIdAsync(Guid paperId);
    }
}