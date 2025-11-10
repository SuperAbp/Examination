using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperQuestions
{
    /// <summary>
    /// 试卷题目
    /// </summary>
    public interface IPaperQuestionRepository : IRepository<PaperQuestion, Guid>
    {
        Task<List<PaperQuestion>> GetListByPaperSectionIdAsync(Guid paperSectionId, CancellationToken cancellationToken = default);

        Task<List<PaperQuestion>> GetListByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);

        Task<PaperQuestion> GetByPaperSectionIdAndQuestionIdAsync(Guid paperSectionId, Guid questionId, CancellationToken cancellationToken = default);
    }
}
