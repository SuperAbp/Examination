using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.EntityFrameworkCore.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
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
            // TODO:change check logic
            return await (await GetQueryableAsync()).AnyAsync(x => x.UserId == userId && x.Status == UserExamStatus.InProgress, cancellationToken);
        }

        public async Task<UserExamWithDetails> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var userExamQueryable = await GetQueryableAsync();
            var examQuestionQueryable = dbContext.Set<UserExamQuestion>().AsQueryable();
            var questionQueryable = dbContext.Set<Question>().AsQueryable();
            var questionAnswerQueryable = dbContext.Set<QuestionAnswer>().AsQueryable();

            UserExam userExam = await userExamQueryable
                 .AsNoTracking()
                 .Include(ue => ue.Questions)
                .SingleAsync(ue => ue.Id == id, cancellationToken);

            return await (from ue in userExamQueryable
                          join ueq in examQuestionQueryable on ue.Id equals ueq.UserExamId
                          join q in questionQueryable on ueq.QuestionId equals q.Id
                          join a in questionAnswerQueryable on q.Id equals a.QuestionId into questionAnswers
                          where ue.Id == id
                          select new UserExamWithDetails()
                          {
                              Id = ue.Id,
                              Answers = ueq.Answers,
                              Right = ueq.Right,
                              Score = ueq.Score,
                              Question = q
                              // QuestionId = q.Id,
                              // Question = q.Content,
                              // QuestionAnalysis = q.Analysis,
                              // QuestionScore = ueq.QuestionScore,
                              // QuestionType = q.QuestionType,
                              // QuestionAnswers = questionAnswers.ToList()
                          }).SingleAsync(cancellationToken);
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
                              UserId = g.Key,
                              TotalCount = g.Count(),
                              MaxScore = g.Max(c => c.TotalScore)
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
                              CreationTime = ue.CreationTime,
                              FinishedTime = ue.FinishedTime,
                              TotalScore = ue.TotalScore,
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
                            (e.EndTime.HasValue && e.EndTime.Value < now)
                            ||
                            (ue.StartTime.HasValue && !ue.FinishedTime.HasValue && ue.StartTime.Value.AddMinutes(e.TotalTime) < now)
                        select ue;
            return await query.ToListAsync(cancellationToken);
        }
    }
}