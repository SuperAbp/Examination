using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SuperAbp.Exam.Favorites;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 我的收藏
/// </summary>
[Route("api/favorite")]
public class FavoriteController(IFavoriteAppService favoriteAppService) : ExamController, IFavoriteAppService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <returns>结果</returns>
    [HttpGet]
    public async Task<PagedResultDto<FavoriteListDto>> GetListAsync(GetFavoritesInput input)
    {
        return await favoriteAppService.GetListAsync(input);
    }

    /// <summary>
    /// 数量
    /// </summary>
    /// <returns>结果</returns>
    [HttpGet("count")]
    public async Task<long> GetCountAsync()
    {
        return await favoriteAppService.GetCountAsync();
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    [HttpGet("question/{questionId}")]
    public async Task<bool> GetByQuestionIdAsync(Guid questionId)
    {
        return await favoriteAppService.GetByQuestionIdAsync(questionId);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    [HttpPost]
    public async Task CreateAsync(Guid questionId)
    {
        await favoriteAppService.CreateAsync(questionId);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="questionId">试题Id</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task DeleteAsync(Guid questionId)
    {
        await favoriteAppService.DeleteAsync(questionId);
    }
}