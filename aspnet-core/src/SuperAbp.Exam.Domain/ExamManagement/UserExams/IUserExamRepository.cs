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
        /// 详情
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserExamWithDetails> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="examId">考试Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            Guid? userId = null,
            Guid? examId = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="sorting">排序</param>
        /// <param name="skipCount">跳过</param>
        /// <param name="maxResultCount">最大</param>
        /// <param name="examId">考试Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserExam>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? examId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="sorting">排序</param>
        /// <param name="skipCount">跳过</param>
        /// <param name="maxResultCount">最大</param>
        /// <param name="userId">用户Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<UserExamWithDetails>> GetListWithDetailAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? userId = null,
            CancellationToken cancellationToken = default);
    }
}