using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Mistakes;

/// <summary>
/// 我的错题
/// </summary>
public interface IMistakeAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<MistakeListDto>> GetListAsync(GetMistakesInput input);
}