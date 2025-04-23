using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.KnowledgePoints;

/// <summary>
/// 问题类别
/// </summary>
public interface IKnowledgePointRepository : IRepository<KnowledgePoint, Guid>
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<KnowledgePoint>> GetListAsync(string? name = null, CancellationToken cancellationToken = default);
}