using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public interface IPaperQuestionRuleRepository : IRepository<PaperQuestionRule, Guid>
    {
        Task<PaperQuestionRule> GetAsync(Guid paperId, Guid questionRepositoryId, CancellationToken cancellationToken = default);

        Task<PaperQuestionRule?> FindAsync(Guid paperId, Guid questionRepositoryId, CancellationToken cancellationToken = default);

        Task<List<PaperQuestionRule>> GetListAsync(
            string? sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? paperId = null,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid paperId, Guid questionRepositoryId, CancellationToken cancellationToken = default);

        Task DeleteByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default);
    }
}