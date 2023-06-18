using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 问题管理
/// </summary>
public interface IQuestionAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input);

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuestionDetailDto> GetAsync(Guid id);
}