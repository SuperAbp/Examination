using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperRepos
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public interface IPaperRepoRepository : IRepository<PaperRepo, Guid>
    {
        Task<PaperRepo> GetAsync(Guid examingId, Guid questionRepositoryId);

        Task<PaperRepo> FindAsync(Guid examingId, Guid questionRepositoryId);

        Task DeleteAsync(Guid examingId, Guid questionRepositoryId);
        Task DeleteByExamingIdAsync(Guid examingId);
    }
}