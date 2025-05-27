using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 题库
    /// </summary>
    public class QuestionBankRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
        : EfCoreRepository<ExamDbContext, QuestionBank, Guid>(dbContextProvider), IQuestionBankRepository
    {
        public async Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetQueryableAsync();
            return await dbSet.AnyAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<string?> FindTitleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .Where(r => r.Id == id)
                .Select(r => r.Title)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(string? title = null, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(title), user => user.Title.Contains(title))
                .CountAsync(cancellationToken);
        }

        public async Task<List<QuestionBank>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            string? title = null,
            CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                 .WhereIf(!title.IsNullOrWhiteSpace(), r => r.Title.Contains(title))
                 .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(QuestionBank.CreationTime) : sorting)
                 .PageBy(skipCount, maxResultCount)
                 .ToListAsync(cancellationToken);
        }

        public async Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetQueryableAsync();
            return await dbSet.AnyAsync(x => x.Title == title, cancellationToken);
        }
    }
}