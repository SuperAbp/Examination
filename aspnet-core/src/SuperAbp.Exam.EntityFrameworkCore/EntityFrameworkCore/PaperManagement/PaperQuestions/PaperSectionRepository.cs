using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.PaperSections;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.PaperQuestions;

public class PaperSectionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, PaperSection, Guid>(dbContextProvider), IPaperSectionRepository
{
    public async Task<List<PaperSection>> GetListByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(p => p.PaperId == paperId)
            .OrderBy(p => p.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaperSection> GetByPaperIdAndOrderAsync(Guid paperId, int order, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .FirstOrDefaultAsync(p => p.PaperId == paperId && p.Order == order, cancellationToken);
    }
}
