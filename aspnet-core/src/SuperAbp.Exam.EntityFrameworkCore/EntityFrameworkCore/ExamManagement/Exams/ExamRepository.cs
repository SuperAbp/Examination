using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public class ExamRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, Examination, Guid>(dbContextProvider), IExamRepository
    {
        public async Task<bool> ExistsByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            return await dbSet.AnyAsync(q => q.PaperId == paperId, cancellationToken);
        }
    }
}