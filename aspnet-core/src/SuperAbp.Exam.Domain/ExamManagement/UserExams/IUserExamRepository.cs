using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 用户考试
    /// </summary>
    public interface IUserExamRepository : IRepository<UserExam, Guid>
    {
        Task<bool> UnfinishedExistsAsync(Guid userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            Guid? userId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="sorting">排序</param>
        /// <param name="skipCount">跳过</param>
        /// <param name="maxResultCount">最大</param>
        /// <param name="userId">用户Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserExamWithDetails>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? userId = null,
            CancellationToken cancellationToken = default);
    }
}