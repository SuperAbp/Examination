using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public class ExamingRepository : EfCoreRepository<ExamDbContext, Examing, Guid>, IExamingRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public ExamingRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await (await GetQueryableAsync()).AnyAsync(e => e.Name == name);
        }
    }
}