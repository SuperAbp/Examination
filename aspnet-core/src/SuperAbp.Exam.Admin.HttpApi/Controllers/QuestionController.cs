using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 问题
/// </summary>
[Route("api/question-management/question")]
public class QuestionController : ExamController, IQuestionAppService
{
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    [HttpGet]
    public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
    {
        return await _questionAppService.GetListAsync(input);
    }

    /// <summary>
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}/editor")]
    public virtual async Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id)
    {
        return await _questionAppService.GetEditorAsync(id);
    }

    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("import")]
    public virtual async Task ImportAsync(QuestionImportDto input)
    {
        await _questionAppService.ImportAsync(input);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual async Task<QuestionListDto> CreateAsync(QuestionCreateDto input)
    {
        return await _questionAppService.CreateAsync(input);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public virtual async Task<QuestionListDto> UpdateAsync(Guid id, QuestionUpdateDto input)
    {
        return await _questionAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _questionAppService.DeleteAsync(id);
    }
}