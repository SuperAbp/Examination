using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 用户考题
    /// </summary>
    public interface IUserExamQuestionRepository : IRepository<UserExamQuestion, Guid>
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="userExamId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserExamQuestionWithDetails>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? userExamId = null,
            CancellationToken cancellationToken = default);
    }
}