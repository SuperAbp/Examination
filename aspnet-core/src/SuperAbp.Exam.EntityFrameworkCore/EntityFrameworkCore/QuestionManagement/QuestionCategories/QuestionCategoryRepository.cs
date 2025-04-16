using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionCategories;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionCategories;

public class QuestionCategoryRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
    : EfCoreRepository<ExamDbContext, QuestionCategory, Guid>(dbContextProvider), IQuestionCategoryRepository
{
    public async Task<int> GetCountAsync(string? name = null, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .CountAsync(cancellationToken);
    }

    public async Task<List<QuestionCategoryWithDetails>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue, string? name = null, Guid? parentId = null, CancellationToken cancellationToken = default)
    {
        return await (from c in await GetQueryableAsync(name, parentId)
                      join p in await GetQueryableAsync() on c.ParentId equals p.Id into parent
                      from p in parent.DefaultIfEmpty()
                      select new QuestionCategoryWithDetails()
                      {
                          Id = c.Id,
                          Name = c.Name,
                          ParentName = p.Name,
                          CreationTime = c.CreationTime
                      })
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? QuestionCategoryConsts.DefaultSorting : sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    protected async Task<IQueryable<QuestionCategory>> GetQueryableAsync(string? name = null, Guid? parentId = null)
    {
        return (await GetDbSetAsync())
            .WhereIf(!name.IsNullOrWhiteSpace(), s => s.Name.Contains(name))
            .WhereIf(parentId.HasValue, s => s.ParentId.HasValue && s.ParentId.Value == parentId.Value);
    }
}