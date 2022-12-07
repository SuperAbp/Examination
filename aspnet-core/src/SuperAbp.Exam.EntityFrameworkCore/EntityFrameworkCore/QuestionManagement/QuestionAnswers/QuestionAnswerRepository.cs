using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using System;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案
    /// </summary>
    public class QuestionAnswerRepository : EfCoreRepository<ExamDbContext, QuestionAnswer, Guid>, IQuestionAnswerRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public QuestionAnswerRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // TODO:编写仓储代码
    }
}