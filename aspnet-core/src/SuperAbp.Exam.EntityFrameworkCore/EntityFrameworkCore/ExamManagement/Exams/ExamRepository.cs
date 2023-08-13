using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public class ExamRepository : EfCoreRepository<ExamDbContext, Examination, Guid>, IExamRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public ExamRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        // TODO:编写仓储代码
    }
}