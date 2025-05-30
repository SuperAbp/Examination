using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.Papers;

/// <summary>
/// 考试
/// </summary>
public class PaperRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, Paper, Guid>(dbContextProvider), IPaperRepository
{
    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(e => e.Name == name, cancellationToken);
    }
}