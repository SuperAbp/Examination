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
    public class QuestionRepoRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
        : EfCoreRepository<ExamDbContext, QuestionRepo, Guid>(dbContextProvider), IQuestionRepoRepository
    {
        public async Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.AnyAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<string?> FindTitleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(r => r.Id == id)
                .Select(r => r.Title)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(string? title = null, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(title), user => user.Title.Contains(title))
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<QuestionRepo>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            string? title = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                 .WhereIf(!title.IsNullOrWhiteSpace(), r => r.Title.Contains(title))
                 .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(QuestionRepo.CreationTime) : sorting)
                 .PageBy(skipCount, maxResultCount)
                 .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.AnyAsync(x => x.Title == title, GetCancellationToken(cancellationToken));
        }
    }
}