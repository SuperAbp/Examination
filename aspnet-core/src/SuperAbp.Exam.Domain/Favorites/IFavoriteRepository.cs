using System;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;
using System.Collections.Generic;

namespace SuperAbp.Exam.Favorites;

/// <summary>
/// 收藏夹
/// </summary>
public interface IFavoriteRepository : IRepository<Favorite, Guid>
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="creatorId">创建人Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<FavoriteWithDetails>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? creatorId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 数量
    /// </summary>
    /// <param name="creatorId">创建人Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> CountAsync(Guid creatorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 存在
    /// </summary>
    /// <param name="creatorId">创建人Id</param>
    /// <param name="questionId">题目Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Guid creatorId, Guid questionId, CancellationToken cancellationToken = default);
}