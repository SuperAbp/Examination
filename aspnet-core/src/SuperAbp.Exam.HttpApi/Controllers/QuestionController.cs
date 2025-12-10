using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

[Route("api/questions")]
public class QuestionController : ExamController, IQuestionAppService
{
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    [HttpGet("{id}")]
    public async Task<QuestionDetailDto> GetAsync(Guid id)
    {
        return await _questionAppService.GetAsync(id);
    }

    [HttpGet("ids")]
    public async Task<ListResultDto<Guid>> GetIdsAsync(GetQuestionsInput input)
    {
        return await _questionAppService.GetIdsAsync(input);
    }

    [HttpGet]
    public async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
    {
        return await _questionAppService.GetListAsync(input);
    }
}