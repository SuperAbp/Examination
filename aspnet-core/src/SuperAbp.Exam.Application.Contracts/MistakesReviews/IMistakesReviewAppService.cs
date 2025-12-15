using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.MistakesReviews;

/// <summary>
/// 我的错题
/// </summary>
public interface IMistakesReviewAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<MistakesReviewListDto>> GetListAsync(GetMistakesReviewInput input);
}