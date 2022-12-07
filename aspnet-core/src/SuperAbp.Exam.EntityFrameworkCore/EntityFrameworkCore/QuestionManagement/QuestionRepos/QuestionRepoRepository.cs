using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using System;

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

        // TODO:编写仓储代码
    }
}