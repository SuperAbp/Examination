using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;

/// <summary>
/// 题目类别
/// </summary>
public interface IQuestionCategoryAdminAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    Task<PagedResultDto<QuestionCategoryListDto>> GetListAsync(GetQuestionCategoriesInput input);

    /// <summary>
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<GetQuestionCategoryForEditorOutput> GetEditorAsync(Guid id);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<Guid> CreateAsync(QuestionCategoryCreateDto input);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, QuestionCategoryUpdateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
}