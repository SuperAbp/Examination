using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExams
{
    /// <summary>
    /// 用户考试
    /// </summary>
    public class UserExamRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, UserExam, Guid>(dbContextProvider), IUserExamRepository
    {
        public async Task<bool> UnfinishedExistsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).AnyAsync(GetUnfinishedFilterExpression(userId), cancellationToken);
        }

        public async Task<UserExam?> GetUnfinishedAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .SingleOrDefaultAsync(GetUnfinishedFilterExpression(userId), cancellationToken);
        }

        private Expression<Func<UserExam, bool>> GetUnfinishedFilterExpression(Guid userId)
        {
            return x => x.UserId == userId && new[] { UserExamStatus.Waiting, UserExamStatus.InProgress }.Contains(x.Status);
        }

        public async Task<List<UserExam>> GetInProgressAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .Where(e => e.ExamId == examId && new[] { UserExamStatus.Waiting, UserExamStatus.InProgress }.Contains(e.Status))
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(Guid? userId = null,
            Guid? examId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync())
                .WhereIf(userId.HasValue, c => c.UserId == userId)
                .WhereIf(examId.HasValue, c => c.ExamId == examId)
                .CountAsync(cancellationToken);
        }

        public async Task<List<UserExam>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue, Guid? examId = null,
            Guid? userId = null,
            CancellationToken cancellationToken = default)
        {
            // TODO: How to combine with GetListWithDetailAsync;
            var queryable = await GetQueryableAsync();
            return await queryable
                .WhereIf(examId.HasValue, c => c.ExamId == examId.Value)
                .WhereIf(userId.HasValue, c => c.UserId == userId.Value)
                .OrderBy(sorting ?? UserExamConsts.DefaultSorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserExamWithUser>> GetListByExamIdAsync(Guid examId, string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var queryable = (await GetQueryableAsync())
                .Where(e => e.ExamId == examId)
                .OrderBy(sorting ?? UserExamConsts.DefaultSorting)
                .Skip(skipCount)
                .Take(maxResultCount);
            return await (from e in queryable
                          where e.ExamId == examId
                          group e by e.UserId into g
                          select new UserExamWithUser()
                          {
                              UserExamId = g.FirstOrDefault().Id,
                              UserId = g.Key,
                              UserName = null,
                              TotalCount = g.Count(),
                              MaxScore = g.Max(c => c.TotalScore),
                              TotalScore = g.Max(c => c.TotalScore),
                              IsPassed = g.FirstOrDefault().IsPassed,
                              FinishedTime = g.FirstOrDefault().FinishedTime
                          })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserExamWithDetails>> GetListWithDetailAsync(string? sorting = null,
            int skipCount = 0,
            int maxResultCount = Int32.MaxValue,
            Guid? userId = null,
            Guid? examId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var queryable = (await GetQueryableAsync())
                .WhereIf(userId.HasValue, c => c.UserId == userId.Value)
                .WhereIf(examId.HasValue, e => e.ExamId == examId.Value);
            var examQueryable = dbContext.Set<Examination>().AsQueryable();
            return await (from ue in queryable
                          join e in examQueryable on ue.ExamId equals e.Id
                          select new UserExamWithDetails()
                          {
                              Id = ue.Id,
                              ExamId = e.Id,
                              ExamName = e.Name,
                              ExamStatus = e.Status,
                              CreationTime = ue.CreationTime,
                              FinishedTime = ue.FinishedTime,
                              TotalScore = ue.TotalScore,
                              IsPassed = ue.IsPassed,
                              Status = ue.Status
                          })
                .OrderBy(sorting ?? UserExamConsts.DefaultSorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserExam>> GetTimeoutUserExamsAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userExamQueryable = await GetQueryableAsync();
            var examQueryable = dbContext.Set<Examination>().AsQueryable();

            var query = from ue in userExamQueryable
                        join e in examQueryable on ue.ExamId equals e.Id
                        where
                            (ue.Status == UserExamStatus.Waiting || ue.Status == UserExamStatus.InProgress) &&
                            (
                                (e.EndTime.HasValue && e.EndTime.Value < now)
                                || (ue.StartTime.HasValue && !ue.FinishedTime.HasValue && ue.StartTime.Value.AddMinutes(e.TotalTime) < now)
                                || (!ue.StartTime.HasValue && ue.CreationTime.AddMinutes(e.TotalTime) < now)
                            )
                        select ue;
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<List<UserExamWithUser>> GetRankingListAsync(Guid examId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userExamQueryable = await GetQueryableAsync();
            var userQueryable = dbContext.Users.AsQueryable();

            var query = from ue in userExamQueryable
                        join u in userQueryable on ue.UserId equals u.Id
                        where ue.ExamId == examId && ue.Status == UserExamStatus.Scored
                        select new UserExamWithUser
                        {
                            UserExamId = ue.Id,
                            UserId = ue.UserId,
                            UserName = u.UserName,
                            TotalScore = ue.TotalScore,
                            IsPassed = ue.IsPassed,
                            FinishedTime = ue.FinishedTime
                        };

            return await query.OrderByDescending(x => x.TotalScore)
                              .ThenBy(x => x.FinishedTime)
                              .ToListAsync(cancellationToken);
        }
    }
}