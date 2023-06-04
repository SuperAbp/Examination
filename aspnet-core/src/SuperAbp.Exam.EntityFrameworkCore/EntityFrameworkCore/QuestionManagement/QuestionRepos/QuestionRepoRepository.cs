using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
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

        public async Task<int> GetCountAsync(string title = null, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(title), user => user.Title.Contains(title))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<QuestionRepo>> GetListAsync(
            string title = null,
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                 .WhereIf(!title.IsNullOrWhiteSpace(), r => r.Title.Contains(title))
                 .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(QuestionRepo.CreationTime) : sorting)
                 .PageBy(skipCount, maxResultCount)
                 .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}