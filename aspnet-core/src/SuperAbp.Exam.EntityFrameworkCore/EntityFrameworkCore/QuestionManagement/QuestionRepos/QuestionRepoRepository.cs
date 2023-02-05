using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库
    /// </summary>
    public class QuestionRepoRepository : EfCoreRepository<ExamDbContext, QuestionRepo, Guid>, IQuestionRepoRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public QuestionRepoRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        public async Task<string> FindTitleAsync(Guid id)
        {
            return await (await GetDbSetAsync())
                .Where(r => r.Id == id)
                .Select(r => r.Title)
                .FirstOrDefaultAsync();
        }
    }
}