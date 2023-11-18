using System;
using System.Collections.Generic;
using System.Threading;
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

        Task<List<PaperRepo>> GetListAsync(
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? paperId = null,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid paperId, Guid questionRepositoryId);

        Task DeleteByExamIdAsync(Guid paperId);
    }
}