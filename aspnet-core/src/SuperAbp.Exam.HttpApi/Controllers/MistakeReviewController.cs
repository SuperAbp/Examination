using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.MistakeReviews;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 我的错题
/// </summary>
[Route("api/mistake-reviews")]
public class MistakeReviewController(IMistakeReviewAppService mistakesReviewAppService) : ExamController, IMistakeReviewAppService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<MistakeReviewListDto>> GetListAsync(GetMistakeReviewsInput input)
    {
        return await mistakesReviewAppService.GetListAsync(input);
    }
}