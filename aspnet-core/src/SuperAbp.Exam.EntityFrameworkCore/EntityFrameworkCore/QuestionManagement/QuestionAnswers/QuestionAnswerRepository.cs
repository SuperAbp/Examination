using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<QuestionAnswer>> GetListAsync(Guid questionId)
        {
            return await GetListAsync(a => a.QuestionId == questionId);
        }

        public async Task<bool> ContentExistsAsync(Guid questionId, string content, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.QuestionId == questionId && x.Content == content, GetCancellationToken(cancellationToken));
        }
    }
}