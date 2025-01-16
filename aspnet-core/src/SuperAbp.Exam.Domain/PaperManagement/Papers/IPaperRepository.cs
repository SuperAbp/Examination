using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.Papers;

/// <summary>
/// 考试
/// </summary>
public interface IPaperRepository : IRepository<Paper, Guid>
{
    Task<bool> NameExistsAsync(string content, CancellationToken cancellationToken = default);
}