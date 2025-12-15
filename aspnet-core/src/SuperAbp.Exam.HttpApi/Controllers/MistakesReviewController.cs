using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.MistakesReviews;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 我的错题
/// </summary>
[Route("api/mistakes-reviews")]
public class MistakesReviewController(IMistakesReviewAppService mistakesReviewAppService) : ExamController, IMistakesReviewAppService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<MistakesReviewListDto>> GetListAsync(GetMistakesReviewInput input)
    {
        return await mistakesReviewAppService.GetListAsync(input);
    }
}