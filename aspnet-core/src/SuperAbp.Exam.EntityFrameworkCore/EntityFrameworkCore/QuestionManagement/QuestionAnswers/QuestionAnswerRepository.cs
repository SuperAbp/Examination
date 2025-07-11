using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案
    /// </summary>
    public class QuestionAnswerRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, QuestionAnswer, Guid>(dbContextProvider), IQuestionAnswerRepository
    {
        public async Task<List<QuestionAnswer>> GetListAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            return await GetListAsync(a => a.QuestionId == questionId, cancellationToken: cancellationToken);
        }

        public async Task<bool> ContentExistsAsync(Guid questionId, string content, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).AnyAsync(x => x.QuestionId == questionId && x.Content == content, cancellationToken);
        }

        public async Task DeleteByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(er => er.QuestionId == questionId, cancellationToken: cancellationToken);
        }
    }
}