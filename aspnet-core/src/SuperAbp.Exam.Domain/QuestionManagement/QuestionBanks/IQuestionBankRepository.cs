using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 题库
    /// </summary>
    public interface IQuestionBankRepository : IRepository<QuestionBank, Guid>
    {
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string?> FindTitleAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 数量
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(
            string? title = null,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="sorting">排序</param>
        /// <param name="skipCount">跳过</param>
        /// <param name="maxResultCount">最大</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<QuestionBank>> GetListAsync(string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            string? title = null,
            CancellationToken cancellationToken = default);

        Task<bool> IdExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default);
    }
}