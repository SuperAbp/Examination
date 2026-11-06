using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Options;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 选项
/// </summary>
[Route("api/options")]
public class OptionController(IOptionAppService optionAppService) : ExamController, IOptionAppService
{
    [HttpGet("question-types")]
    public async Task<Dictionary<int, string>> GetQuestionTypes()
    {
        return await optionAppService.GetQuestionTypes();
    }

    [HttpGet("answer-modes")]
    public async Task<Dictionary<int, string>> GetAnswerModes()
    {
        return await optionAppService.GetAnswerModes();
    }

    [HttpGet("review-modes")]
    public async Task<Dictionary<int, string>> GetReviewModes()
    {
        return await optionAppService.GetReviewModes();
    }

    [HttpGet("examination-status")]
    public async Task<Dictionary<int, string>> GetExaminationStatus()
    {
        return await optionAppService.GetExaminationStatus();
    }

    [HttpGet("user-exam-status")]
    public async Task<Dictionary<int, string>> GetUserExamStatus()
    {
        return await optionAppService.GetUserExamStatus();
    }
}

