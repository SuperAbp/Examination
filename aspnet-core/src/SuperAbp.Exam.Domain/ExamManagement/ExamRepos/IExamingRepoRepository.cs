using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public interface IExamingRepoRepository : IRepository<ExamingRepo, Guid>
    {
        Task<ExamingRepo> GetAsync(Guid examingId, Guid questionRepositoryId);

        Task<ExamingRepo> FindAsync(Guid examingId, Guid questionRepositoryId);

        Task DeleteAsync(Guid examingId, Guid questionRepositoryId);
        Task DeleteByExamingIdAsync(Guid examingId);
    }
}