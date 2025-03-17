using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Favorites;

public interface IFavoriteAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <returns>结果</returns>
    Task<ListResultDto<FavoriteListDto>> GetListAsync();

    /// <summary>
    /// 数量
    /// </summary>
    /// <returns>结果</returns>
    Task<int> GetCountAsync();

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task<bool> GetByQuestionIdAsync(Guid questionId);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task CreateAsync(Guid questionId);

    Task DeleteAsync(Guid questionId);
}