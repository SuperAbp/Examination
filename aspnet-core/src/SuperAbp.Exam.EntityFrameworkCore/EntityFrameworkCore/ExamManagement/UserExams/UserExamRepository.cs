using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.UserExams;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExams
{
    /// <summary>
    /// 用户考试
    /// </summary>
    public class UserExamRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
        : EfCoreRepository<ExamDbContext, UserExam, Guid>(dbContextProvider), IUserExamRepository
    {
        public async Task<bool> AnyByExamIdAndUserIdAsync(Guid examId, Guid userId, CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.AnyAsync(ue => ue.ExamId == examId && ue.UserId == userId, cancellationToken);
        }
    }
}