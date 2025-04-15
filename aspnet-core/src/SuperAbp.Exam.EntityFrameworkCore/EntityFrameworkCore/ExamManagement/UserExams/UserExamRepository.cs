using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
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
        public async Task<bool> UnfinishedExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).AnyAsync(x => x.UserId == userId && !x.Finished, GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetCountAsync(Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).WhereIf(userId.HasValue, c => c.UserId == userId).CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<UserExamWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue, Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var queryable = (await GetQueryableAsync()).WhereIf(userId.HasValue, c => c.UserId == userId);
            var examQueryable = dbContext.Set<Examination>().AsQueryable();
            return await (from ue in queryable
                          join e in examQueryable on ue.ExamId equals e.Id
                          select new UserExamWithDetails()
                          {
                              Id = ue.Id,
                              ExamId = e.Id,
                              ExamName = e.Name,
                              CreationTime = ue.CreationTime,
                              FinishedTime = ue.FinishedTime,
                              TotalScore = ue.TotalScore,
                              Finished = ue.Finished
                          }).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}