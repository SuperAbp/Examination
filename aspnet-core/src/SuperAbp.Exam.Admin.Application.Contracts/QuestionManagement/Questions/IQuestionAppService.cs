using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

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
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id);

    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ImportAsync(QuestionImportDto input);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<QuestionListDto> CreateAsync(QuestionCreateDto input);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<QuestionListDto> UpdateAsync(Guid id, QuestionUpdateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
}