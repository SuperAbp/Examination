using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
    public class UserExamRepository : EfCoreRepository<ExamDbContext, UserExam, Guid>, IUserExamRepository
    {
        /// <summary>
        /// .ctor
        ///</summary>
        public UserExamRepository(
            IDbContextProvider<ExamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> AnyByExamIdAndUserIdAsync(Guid examId, Guid userId)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.AnyAsync(ue => ue.ExamId == examId && ue.UserId == userId);
        }
    }
}