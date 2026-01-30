using System;
using System.Collections.Generic;
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

    /// <summary>
    /// 获取包含指定题目的试卷列表
    /// </summary>
    Task<List<Paper>> GetPapersByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
}