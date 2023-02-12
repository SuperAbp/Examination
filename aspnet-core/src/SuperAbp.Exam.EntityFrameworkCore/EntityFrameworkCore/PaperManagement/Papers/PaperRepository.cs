using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.Papers;

/// <summary>
/// 考试
/// </summary>
public class PaperRepository : EfCoreRepository<ExamDbContext, Paper, Guid>, IPaperRepository
{
    /// <summary>
    /// .ctor
    ///</summary>
    public PaperRepository(
        IDbContextProvider<ExamDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
        
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await (await GetQueryableAsync()).AnyAsync(e => e.Name == name);
    }
}