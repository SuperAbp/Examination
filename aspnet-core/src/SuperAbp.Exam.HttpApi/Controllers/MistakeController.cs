using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Mistakes;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 我的错题
/// </summary>
[Route("api/mistakes")]
public class MistakeController(IMistakeAppService mistakesReviewAppService) : ExamController, IMistakeAppService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<MistakeListDto>> GetListAsync(GetMistakesInput input)
    {
        return await mistakesReviewAppService.GetListAsync(input);
    }
}