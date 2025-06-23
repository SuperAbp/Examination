using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public class ExamRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, Examination, Guid>(dbContextProvider), IExamRepository
    {
        // TODO:编写仓储代码
    }
}