using SuperAbp.Exam.MistakeReviews;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.MistakeReviews;

/// <summary>
/// 我的错题
/// </summary>
public interface IMistakeReviewAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<MistakeReviewListDto>> GetListAsync(GetMistakeReviewsInput input);
}