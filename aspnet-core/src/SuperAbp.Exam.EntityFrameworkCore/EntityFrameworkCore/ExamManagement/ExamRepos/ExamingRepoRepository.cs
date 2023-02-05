using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.ExamRepos;
using System;
using System.Threading.Tasks;
using SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.Questions;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public class ExamingRepoRepository : EfCoreRepository<ExamDbContext, ExamingRepo, Guid>, IExamingRepoRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public ExamingRepoRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<ExamingRepo> GetAsync(Guid examingId, Guid questionRepositoryId)
        {
            return GetAsync(er => er.ExamingId == examingId
                && er.QuestionRepositoryId == questionRepositoryId);
        }

        public Task<ExamingRepo> FindAsync(Guid examingId, Guid questionRepositoryId)
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
}