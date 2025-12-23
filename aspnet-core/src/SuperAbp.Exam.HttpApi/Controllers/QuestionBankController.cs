using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 题库管理
/// </summary>
[Route("api/question-banks")]
public class QuestionBankController : ExamController, IQuestionBankAppService
{
    private readonly IQuestionBankAppService _questionBankAppService;

    public QuestionBankController(IQuestionBankAppService questionBankAppService)
    {
        _questionBankAppService = questionBankAppService;
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">题库Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<QuestionBankDetailDto> GetAsync(Guid id)
    {
        return await _questionBankAppService.GetAsync(id);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
    {
        return await _questionBankAppService.GetListAsync(input);
    }

    /// <summary>
    /// 题型
    /// </summary>
    /// <param name="id">题库Id</param>
    /// <returns></returns>
    [HttpGet("{id}/question-types")]
    public async Task<ListResultDto<int>> GetQuestionTypesAsync(Guid id)
    {
        return await _questionBankAppService.GetQuestionTypesAsync(id);
    }
}