using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.Papers;

/// <summary>
/// 试卷
/// </summary>
public interface IPaperRepository : IRepository<Paper, Guid>
{
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task RemovePaperQuestionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
}