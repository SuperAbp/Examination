using System.Collections.Generic;
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
    public Dictionary<int, string> GetQuestionTypes()
    {
        return optionAppService.GetQuestionTypes();
    }

    [HttpGet("answer-modes")]
    public Dictionary<int, string> GetAnswerModes()
    {
        return optionAppService.GetAnswerModes();
    }

    [HttpGet("review-modes")]
    public Dictionary<int, string> GetReviewModes()
    {
        return optionAppService.GetReviewModes();
    }

    [HttpGet("examination-status")]
    public Dictionary<int, string> GetExaminationStatus()
    {
        return optionAppService.GetExaminationStatus();
    }

    [HttpGet("user-exam-status")]
    public Dictionary<int, string> GetUserExamStatus()
    {
        return optionAppService.GetUserExamStatus();
    }
}