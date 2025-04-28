using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.KnowledgePoints;

/// <summary>
/// 知识点
/// </summary>
public interface IKnowledgePointRepository : IRepository<KnowledgePoint, Guid>
{
    /// <summary>
    /// 根据问题Id获取知识点
    /// </summary>
    /// <param name="questionId">问题Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<KnowledgePoint>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<KnowledgePoint>> GetListAsync(string? name = null, CancellationToken cancellationToken = default);
}