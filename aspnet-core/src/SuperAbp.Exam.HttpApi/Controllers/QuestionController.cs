using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 问题管理
/// </summary>
[Route("api/questions")]
public class QuestionController : ExamController, IQuestionAppService
{
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<QuestionDetailDto> GetAsync(Guid id)
    {
        return await _questionAppService.GetAsync(id);
    }

    /// <summary>
    /// 获取所有Id
    /// </summary>
    /// <param name="input">过滤条件</param>
    /// <returns></returns>
    [HttpGet("ids")]
    public async Task<ListResultDto<Guid>> GetIdsAsync(GetQuestionsInput input)
    {
        return await _questionAppService.GetIdsAsync(input);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    [HttpGet]
    public async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
    {
        return await _questionAppService.GetListAsync(input);
    }
}