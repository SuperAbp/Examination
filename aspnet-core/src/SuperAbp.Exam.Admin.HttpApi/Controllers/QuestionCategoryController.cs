using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 题目类别
/// </summary>
public class QuestionCategoryController(IQuestionCategoryAdminAppService questionCategoryAppService)
    : ExamController, IQuestionCategoryAdminAppService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    [HttpGet]
    public async Task<PagedResultDto<QuestionCategoryListDto>> GetListAsync(GetQuestionCategoriesInput input)
    {
        return await questionCategoryAppService.GetListAsync(input);
    }

    /// <summary>
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}/editor")]
    public async Task<GetQuestionCategoryForEditorOutput> GetEditorAsync(Guid id)
    {
        return await questionCategoryAppService.GetEditorAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> CreateAsync(QuestionCategoryCreateDto input)
    {
        return await questionCategoryAppService.CreateAsync(input);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, QuestionCategoryUpdateDto input)
    {
        await questionCategoryAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await questionCategoryAppService.DeleteAsync(id);
    }
}