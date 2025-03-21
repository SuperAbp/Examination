using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Favorites;

/// <summary>
/// 我的收藏
/// </summary>
public interface IFavoriteAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <returns>结果</returns>
    Task<PagedResultDto<FavoriteListDto>> GetListAsync(GetFavoritesInput input);

    /// <summary>
    /// 数量
    /// </summary>
    /// <returns>结果</returns>
    Task<long> GetCountAsync();

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    Task<bool> GetByQuestionIdAsync(Guid questionId);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    Task CreateAsync(Guid questionId);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    Task DeleteAsync(Guid questionId);
}